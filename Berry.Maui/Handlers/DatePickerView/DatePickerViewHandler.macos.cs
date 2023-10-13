using Microsoft.Maui.Handlers;
using UIKit;

namespace Berry.Maui.Handlers;

public partial class DatePickerViewHandler : DatePickerHandler
{
    protected override UIDatePicker CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        nativeView.Alpha = 0f;

        return nativeView;
    }
}
