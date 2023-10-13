using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Berry.Maui.Handlers;

public partial class TimePickerViewHandler : TimePickerHandler
{
    protected override MauiTimePicker CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        nativeView.BorderStyle = UITextBorderStyle.None;

        return nativeView;
    }
}
