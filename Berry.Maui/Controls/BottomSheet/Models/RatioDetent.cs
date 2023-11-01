using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

[ContentProperty(nameof(Ratio))]
public partial class RatioDetent : Detent
{
#pragma warning disable CS0169
    [AutoBindable]
    readonly float ratio;
#pragma warning restore CS0169
    public override double GetHeight(BottomSheet page, double maxSheetHeight)
    {
        return maxSheetHeight * Ratio;
    }
}
