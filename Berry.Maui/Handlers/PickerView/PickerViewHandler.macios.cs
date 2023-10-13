using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Berry.Maui.Handlers;

public partial class PickerViewHandler : PickerHandler
{
    protected override MauiPicker CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        nativeView.BorderStyle = UITextBorderStyle.None;

        return nativeView;
    }
}
