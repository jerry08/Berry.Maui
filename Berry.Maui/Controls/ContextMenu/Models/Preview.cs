using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Berry.Maui.Controls;

public partial class Preview : Element
{
    [AutoBindable]
    DataTemplate previewTemplate;

    [AutoBindable]
    IShape visiblePath;

    [AutoBindable]
    Color backgroundColor;

    [AutoBindable]
    Thickness padding = new();
}
