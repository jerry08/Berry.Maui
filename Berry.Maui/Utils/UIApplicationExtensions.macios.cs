using System;
using Microsoft.Maui;
using UIKit;

namespace Berry.Maui.Utils;

// Copied from https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/iOS/UIApplicationExtensions.cs
public static class UIApplicationExtensions
{
    public static UIEdgeInsets GetSafeAreaInsetsForWindow(this UIApplication application)
    {
        UIEdgeInsets safeAreaInsets;

        if (!OperatingSystem.IsIOSVersionAtLeast(11))
            safeAreaInsets = new UIEdgeInsets(
                UIApplication.SharedApplication.StatusBarFrame.Size.Height,
                0,
                0,
                0
            );
#pragma warning disable CA1422 // Validate platform compatibility
        else if (application.GetKeyWindow() is UIWindow keyWindow)
            safeAreaInsets = keyWindow.SafeAreaInsets;
        else if (application.Windows.Length > 0)
            safeAreaInsets = application.Windows[0].SafeAreaInsets;
        else
            safeAreaInsets = UIEdgeInsets.Zero;
#pragma warning restore CA1422 // Validate platform compatibility

        return safeAreaInsets;
    }

    public static UIWindow? GetKeyWindow(this UIApplication application)
    {
#pragma warning disable CA1422 // Validate platform compatibility
        var windows = application.Windows;
#pragma warning restore CA1422 // Validate platform compatibility

        for (var i = 0; i < windows.Length; i++)
        {
            var window = windows[i];
            if (window.IsKeyWindow)
                return window;
        }

        return null;
    }

    public static IWindow? GetWindow(this UIApplication application) =>
        application.GetKeyWindow().GetWindow();

    public static IWindow? GetWindow(this UIWindow? platformWindow)
    {
        if (platformWindow is null)
            return null;

        foreach (
            var window in IPlatformApplication.Current?.Application?.Windows
                ?? Array.Empty<IWindow>()
        )
        {
            if (window?.Handler?.PlatformView == platformWindow)
                return window;
        }

        return null;
    }

    public static IWindow? GetWindow(this UIWindowScene? windowScene)
    {
        if (windowScene is null)
            return null;
        foreach (var window in windowScene.Windows)
        {
            var managedWindow = window.GetWindow();

            if (managedWindow is not null)
                return managedWindow;
        }

        if (!OperatingSystem.IsIOSVersionAtLeast(13))
            return null;
        else if (windowScene.Delegate is IUIWindowSceneDelegate sd)
            return sd.GetWindow().GetWindow();

        return null;
    }
}
