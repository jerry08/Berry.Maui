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

    public EndIconMode EndIconMode
    {
        get => (EndIconMode)GetValue(EndIconModeProperty);
        set => SetValue(EndIconModeProperty, value);
    }

    public static readonly BindableProperty EndIconModeProperty = BindableProperty.Create(
        nameof(EndIconMode),
        typeof(EndIconMode),
        typeof(MaterialEntry),
        EndIconMode.None
    );

    public BoxBackgroundMode BoxBackgroundMode
    {
        get => (BoxBackgroundMode)GetValue(BoxBackgroundModeProperty);
        set => SetValue(BoxBackgroundModeProperty, value);
    }

    public static readonly BindableProperty BoxBackgroundModeProperty = BindableProperty.Create(
        nameof(BoxBackgroundMode),
        typeof(BoxBackgroundMode),
        typeof(MaterialEntry),
        BoxBackgroundMode.Filled
    );
}
