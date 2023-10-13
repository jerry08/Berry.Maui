using System.Collections.ObjectModel;
using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

[ContentProperty(nameof(Children))]
public partial class Group : MenuElement
{
    [AutoBindable]
    ObservableCollection<MenuElement> children;

    public Group()
        : base()
    {
        Children = new ObservableCollection<MenuElement>();
    }
}
