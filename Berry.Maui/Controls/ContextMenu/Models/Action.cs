using System.Windows.Input;
using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

public partial class Action : MenuElement
{
    [AutoBindable]
    readonly ICommand command;

    [AutoBindable]
    readonly object commandParameter;

    [AutoBindable]
    readonly ImageSource icon;

    [AutoBindable]
    readonly string systemIcon;

    [AutoBindable(DefaultValue = "true")]
    readonly bool isEnabled;

    [AutoBindable(DefaultValue = "true")]
    readonly bool isVisible;

    [AutoBindable]
    readonly bool isDestructive;

    [AutoBindable]
    readonly string subTitle;
}
