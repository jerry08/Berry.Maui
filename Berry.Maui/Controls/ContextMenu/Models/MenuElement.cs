using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

public partial class MenuElement : Element
{
    [AutoBindable]
    readonly string title;
}
