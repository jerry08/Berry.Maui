using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Berry.Maui.Controls;

public interface IAcrylicView : IContentView
{
    Thickness CornerRadius { get; set; }

    Color TintColor { get; set; }

    Color BorderColor { get; }

    double TintOpacity { get; set; }

    EffectStyle EffectStyle { get; set; }

    Thickness BorderThickness { get; set; }
}
