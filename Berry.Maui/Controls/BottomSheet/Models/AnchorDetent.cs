using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

public partial class AnchorDetent : Detent
{
    double _height = 0;

    [AutoBindable]
    private readonly VisualElement? _anchor;

    public override double GetHeight(BottomSheet page, double maxSheetHeight)
    {
        UpdateHeight(page, maxSheetHeight);
        return _height;
    }

    partial void UpdateHeight(BottomSheet page, double maxSheetHeight);
}
