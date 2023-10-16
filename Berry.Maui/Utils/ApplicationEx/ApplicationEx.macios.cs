using Microsoft.Maui.ApplicationModel;
using UIKit;

namespace Berry.Maui;

public static class ApplicationEx
{
    /// <summary>
    /// Checks if the app it fully visible (active) and running. (Foreground)
    /// </summary>
    public static bool IsInForeground()
    {
        return UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active;
    }

    public static bool IsInBackground()
    {
        return UIApplication.SharedApplication.ApplicationState == UIApplicationState.Background;
    }

    /// <summary>
    /// Checks if the app is running.
    /// </summary>
    public static bool IsRunning()
    {
        if (IsInForeground())
            return true;

        return UIApplication.SharedApplication.ApplicationState == UIApplicationState.Inactive;
    }

    public static int GetStatusBarHeight() =>
        //(int)UIApplication.SharedApplication.StatusBarFrame.Height;
        (int)
            Platform
                .GetCurrentUIViewController()!
                .View!.Window.WindowScene!.StatusBarManager!.StatusBarFrame.Height;

    public static int GetNavigationBarHeight() =>
        (int)
            Platform.GetCurrentUIViewController()!.NavigationController!.NavigationBar.Frame.Height;

    //public static int GetNavigationBarHeight()
    //{
    //    //var navigationBar = UIApplication.SharedApplication.KeyWindow.RootViewController.View.Subviews[0].Subviews.OfType<UINavigationBar>().FirstOrDefault();
    //    //
    //    //if (navigationBar != null)
    //    //{
    //    //    //return (int?)navigationBar.GetNavigationController()?.NavigationBar?.Frame.Size.Height.Value ?? 0;
    //    //}
    //
    //    return (int?)
    //            UIApplication.SharedApplication
    //                ?.KeyWindow?.GetNavigationController()
    //                ?.NavigationBar?.Frame.Size.Height.Value ?? 0;
    //}
    //
    //public static int GetStatusBarHeight()
    //{
    //    return (int?)
    //            UIApplication.SharedApplication
    //                ?.KeyWindow?.GetNavigationController()
    //                ?.NavigationBar?.Frame.Y ?? 0;
    //}
}
