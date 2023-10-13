using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Berry.Maui.Handlers;

public partial class EntryViewHandler : EntryHandler
{
    protected override MauiTextField CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        nativeView.BorderStyle = UIKit.UITextBorderStyle.None;

        return nativeView;
    }
}
