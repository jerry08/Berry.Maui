using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Service.Notification;
using Android.Views;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using APoint = Android.Graphics.Point;
using ARect = Android.Graphics.Rect;

namespace Berry.Maui;

public static partial class ApplicationEx
{
    static APoint realSize = new();
    static APoint displaySize = new();
    static ARect contentRect = new();

    public static partial void SetOrientation(DisplayOrientation orientation)
    {
        if (
            Platform.CurrentActivity is null
            || Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread
        )
        {
            return;
        }

        Platform.CurrentActivity.RequestedOrientation = orientation switch
        {
            DisplayOrientation.Landscape => ScreenOrientation.Landscape,
            DisplayOrientation.Portrait => ScreenOrientation.Portrait,
            _ => ScreenOrientation.Unspecified
        };
    }

    /// <summary>
    /// Checks if the app it fully visible (active) and running. (Foreground)
    /// </summary>
    public static bool IsInForeground()
    {
        var appProcessInfo = new ActivityManager.RunningAppProcessInfo();
        ActivityManager.GetMyMemoryState(appProcessInfo);
        return appProcessInfo.Importance is Importance.Foreground or Importance.Visible;
    }

    public static bool IsInBackground()
    {
        var appProcessInfo = new ActivityManager.RunningAppProcessInfo();
        ActivityManager.GetMyMemoryState(appProcessInfo);
        return appProcessInfo.Importance == Importance.Background;
    }

    public static bool IsRunning()
    {
        var appProcessInfo = new ActivityManager.RunningAppProcessInfo();
        ActivityManager.GetMyMemoryState(appProcessInfo);
        return appProcessInfo.Importance
            is Importance.Foreground
                //or Importance.ForegroundService
                or Importance.Visible;
    }

    public static Notification? GetActiveNotification(Context? context, int id) =>
        GetActiveStatusBarNotification(context, id)?.Notification;

    public static StatusBarNotification? GetActiveStatusBarNotification(Context? context, int id) =>
        GetActiveStatusBarNotifications(context)?.Where(x => x.Id == id).FirstOrDefault();

    public static Notification[] GetActiveNotifications(Context? context) =>
        GetActiveStatusBarNotifications(context)
            .Where(x => x is not null)
            .Select(x => x.Notification!)
            .ToArray() ?? Array.Empty<Notification>();

    public static StatusBarNotification[] GetActiveStatusBarNotifications(Context? context)
    {
        var emptyArray = Array.Empty<StatusBarNotification>();

        if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            return emptyArray;

        if (context is null)
            return emptyArray;

        var notificationManager = NotificationManager.FromContext(context);
        if (notificationManager is null)
            return emptyArray;

#pragma warning disable CA1416
        var barNotifications = notificationManager.GetActiveNotifications();
#pragma warning restore CA1416

        return barNotifications?.ToArray() ?? emptyArray;
    }

    // https://stackoverflow.com/questions/48276053/how-can-i-get-status-bar-height-in-xamarin-forms
    public static int GetStatusBarHeight()
    {
        var statusBarHeight = -1;
        var resourceId =
            Platform.CurrentActivity?.Resources?.GetIdentifier(
                "status_bar_height",
                "dimen",
                "android"
            ) ?? 0;
        if (resourceId > 0)
        {
            statusBarHeight = Platform.CurrentActivity!.Resources!.GetDimensionPixelSize(
                resourceId
            );
        }
        return statusBarHeight;
    }

    public static int GetNavigationBarHeight()
    {
        var navigationBarHeight = -1;
        var resourceId =
            Platform.CurrentActivity?.Resources?.GetIdentifier(
                "navigation_bar_height",
                "dimen",
                "android"
            ) ?? 0;
        if (resourceId > 0)
        {
            navigationBarHeight = Platform.CurrentActivity!.Resources!.GetDimensionPixelSize(
                resourceId
            );
        }
        return navigationBarHeight;
    }

    //public static int GetNavigationBarHeight() =>
    //    GetNavigationBarHeight(Platform.CurrentActivity?.WindowManager);

    public static int GetNavigationBarHeight(IWindowManager? windowManager)
    {
        ArgumentNullException.ThrowIfNull(windowManager);

        int windowWidth;
        int windowHeight;

        if (OperatingSystem.IsAndroidVersionAtLeast((int)BuildVersionCodes.R))
        {
            var windowMetrics = windowManager.CurrentWindowMetrics;
            var windowInsets = windowMetrics.WindowInsets.GetInsetsIgnoringVisibility(
                WindowInsets.Type.SystemBars()
            );
            windowWidth = windowMetrics.Bounds.Width();
            windowHeight = windowMetrics.Bounds.Height();
            return windowHeight < windowWidth
                ? windowInsets.Left + windowInsets.Right
                : windowInsets.Bottom;
        }
        else if (windowManager.DefaultDisplay is null)
        {
            throw new InvalidOperationException(
                $"{nameof(IWindowManager)}.{nameof(IWindowManager.DefaultDisplay)} cannot be null"
            );
        }
        else
        {
            windowManager.DefaultDisplay.GetRealSize(realSize);
            windowManager.DefaultDisplay.GetSize(displaySize);

            return realSize.Y < realSize.X
                ? (realSize.X - displaySize.X)
                : (realSize.Y - displaySize.Y);
        }
    }

    //public static int GetStatusBarHeight() =>
    //    GetStatusBarHeight(
    //        Platform.CurrentActivity?.WindowManager,
    //        (ViewGroup?)Platform.CurrentActivity?.Window?.DecorView
    //    );

    public static int GetStatusBarHeight(IWindowManager? windowManager, ViewGroup? decorView)
    {
        ArgumentNullException.ThrowIfNull(windowManager);
        ArgumentNullException.ThrowIfNull(decorView);

        if (OperatingSystem.IsAndroidVersionAtLeast((int)BuildVersionCodes.R))
        {
            var windowMetrics = windowManager.CurrentWindowMetrics;
            var windowInsets = windowMetrics.WindowInsets.GetInsetsIgnoringVisibility(
                WindowInsets.Type.SystemBars()
            );
            return windowInsets.Top;
        }
        else if (windowManager.DefaultDisplay is null)
        {
            throw new InvalidOperationException(
                $"{nameof(IWindowManager)}.{nameof(IWindowManager.DefaultDisplay)} cannot be null"
            );
        }
        else
        {
            decorView.GetWindowVisibleDisplayFrame(contentRect);
            return contentRect.Top;
        }
    }

    public static Size GetWindowSize(IWindowManager? windowManager, ViewGroup decorView)
    {
        ArgumentNullException.ThrowIfNull(windowManager);

        int windowWidth;
        int windowHeight;
        int statusBarHeight;
        int navigationBarHeight;

        if (OperatingSystem.IsAndroidVersionAtLeast((int)BuildVersionCodes.R))
        {
            var windowMetrics = windowManager.CurrentWindowMetrics;
            var windowInsets = windowMetrics.WindowInsets.GetInsetsIgnoringVisibility(
                WindowInsets.Type.SystemBars()
            );
            windowWidth = windowMetrics.Bounds.Width();
            windowHeight = windowMetrics.Bounds.Height();
            statusBarHeight = windowInsets.Top;
            navigationBarHeight =
                windowHeight < windowWidth
                    ? windowInsets.Left + windowInsets.Right
                    : windowInsets.Bottom;
        }
        else if (windowManager.DefaultDisplay is null)
        {
            throw new InvalidOperationException(
                $"{nameof(IWindowManager)}.{nameof(IWindowManager.DefaultDisplay)} cannot be null"
            );
        }
        else
        {
            windowManager.DefaultDisplay.GetRealSize(realSize);
            windowManager.DefaultDisplay.GetSize(displaySize);
            decorView.GetWindowVisibleDisplayFrame(contentRect);

            windowWidth = realSize.X;
            windowHeight = realSize.Y;
            statusBarHeight = contentRect.Top;

            navigationBarHeight =
                realSize.Y < realSize.X
                    ? (realSize.X - displaySize.X)
                    : (realSize.Y - displaySize.Y);
        }

        windowWidth = (windowWidth - (windowHeight < windowWidth ? navigationBarHeight : 0));
        windowHeight = (
            windowHeight
            - ((windowHeight < windowWidth ? 0 : navigationBarHeight) + statusBarHeight)
        );

        return new Size(windowWidth, windowHeight);
    }
}
