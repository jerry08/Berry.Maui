using Foundation;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Graphics;
using UIKit;

namespace Berry.Maui;

public class StatusBarStyleManager : IStatusBarStyleManager
{
    public void SetDefault()
    {
        SetColoredStatusBar("#00FFFFFF", false);
    }

    public void SetColoredStatusBar(string hexColor, bool isLight)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var statusBar =
                UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = Color.FromHex(hexColor).ToUIColor();
            }
            UIApplication.SharedApplication.SetStatusBarStyle(
                isLight ? UIStatusBarStyle.DarkContent : UIStatusBarStyle.LightContent,
                false
            );
            GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
        });
    }

    public void SetWhiteStatusBar()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var statusBar =
                UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = UIColor.White;
            }
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
            GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
        });
    }

    UIViewController GetCurrentViewController()
    {
        var window = UIApplication.SharedApplication.KeyWindow;
        var vc = window.RootViewController;
        while (vc.PresentedViewController != null)
            vc = vc.PresentedViewController;
        return vc;
    }
}
