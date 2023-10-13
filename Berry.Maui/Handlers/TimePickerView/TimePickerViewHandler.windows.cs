using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Berry.Maui.Handlers;

public partial class TimePickerViewHandler : TimePickerHandler
{
    protected override TimePicker CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        nativeView.BorderThickness = new Thickness(0);

        return nativeView;
    }
}
