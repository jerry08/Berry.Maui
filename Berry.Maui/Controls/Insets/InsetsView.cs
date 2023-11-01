using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

public class InsetsView : ContentView
{
    public InsetsView()
    {
        SetBinding(
            PaddingProperty,
            new Binding(nameof(Insets.InsetsThickness), source: Insets.Current)
        );
    }
}

public class TopInsetView : ContentView
{
    public TopInsetView()
    {
        SetBinding(
            PaddingProperty,
            new Binding(nameof(Insets.TopInsetThickness), source: Insets.Current)
        );
    }
}

public class BottomInsetView : ContentView
{
    public BottomInsetView()
    {
        SetBinding(
            PaddingProperty,
            new Binding(nameof(Insets.BottomInsetThickness), source: Insets.Current)
        );
    }
}
