using System.Collections.ObjectModel;
using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

[ContentProperty(nameof(Children))]
public partial class Menu : MenuElement
{
    [AutoBindable]
    readonly ObservableCollection<MenuElement> children;

    public Menu()
    {
        Children = new ObservableCollection<MenuElement>();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        foreach (var item in Children)
        {
            SetInheritedBindingContext(item, BindingContext);
        }
    }
}
