using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

public abstract partial class Detent : BindableObject
{
#pragma warning disable CS0169
    [AutoBindable(DefaultValue = "true")]
    readonly bool isEnabled;

    [AutoBindable]
    readonly bool isDefault;
#pragma warning restore CS0169
    public abstract double GetHeight(BottomSheet page, double maxSheetHeight);
}
