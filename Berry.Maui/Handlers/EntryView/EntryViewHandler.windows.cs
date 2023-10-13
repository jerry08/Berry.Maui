using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace Berry.Maui.Handlers;

public partial class EntryViewHandler : EntryHandler
{
    protected override TextBox CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        nativeView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        nativeView.Style = null;
        return nativeView;
    }
}
