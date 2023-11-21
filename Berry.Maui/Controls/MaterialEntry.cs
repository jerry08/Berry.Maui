using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

public class MaterialEntry : Entry
{
    public Thickness BoxCornerRadii
    {
        get => (Thickness)GetValue(BoxCornerRadiiProperty);
        set => SetValue(BoxCornerRadiiProperty, value);
    }

    public static readonly BindableProperty BoxCornerRadiiProperty = BindableProperty.Create(
        nameof(BoxCornerRadii),
        typeof(Thickness),
        typeof(MaterialEntry),
        new Thickness(30, 30, 30, 30)
    );
}
