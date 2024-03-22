using System;
using System.Runtime.Versioning;
using Android.App;
using Android.OS;
using AndroidX.Core.View;
using Berry.Maui.Extensions;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;

namespace Berry.Maui.Behaviors;

[SupportedOSPlatform("Android23.0")] // NavigationBar is only supported on Android 23.0+
static partial class NavigationBar
{
    static readonly Lazy<bool> isSupportedHolder =
        new(() =>
        {
            if (OperatingSystem.IsAndroidVersionAtLeast((int)BuildVersionCodes.M))
            {
                return true;
            }

            System.Diagnostics.Trace.WriteLine(
                $"{nameof(NavigationBar)} Color + Style functionality is not supported on this version of the Android operating system. Minimum supported Android API is {BuildVersionCodes.M}"
            );

            return false;
        });

    static Activity Activity =>
        Platform.CurrentActivity
        ?? throw new InvalidOperationException("Android Activity can't be null.");

    static bool IsSupported => isSupportedHolder.Value;

    static void PlatformSetColor(Color color)
    {
        if (IsSupported)
        {
            Activity.Window?.SetNavigationBarColor(color.ToPlatform());
        }
    }

    static void PlatformSetStyle(NavigationBarStyle style)
    {
        if (!IsSupported)
        {
            return;
        }

        switch (style)
        {
            case NavigationBarStyle.DarkContent:
                SetNavigationBarAppearance(Activity, true);
                break;

            case NavigationBarStyle.Default:
            case NavigationBarStyle.LightContent:
                SetNavigationBarAppearance(Activity, false);
                break;

            default:
                throw new NotSupportedException(
                    $"{nameof(NavigationBarStyle)} {style} is not yet supported on Android"
                );
        }
    }

    static void SetNavigationBarAppearance(Activity activity, bool isLightNavigationBars)
    {
        var window = activity.GetCurrentWindow();
        var windowController = WindowCompat.GetInsetsController(window, window.DecorView);
        windowController.AppearanceLightNavigationBars = isLightNavigationBars;
    }
}
