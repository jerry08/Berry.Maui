using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace Berry.Maui.Handlers;

public partial class PickerViewHandler : PickerHandler
{
    protected override ComboBox CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        nativeView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);

        return nativeView;
    }
}
