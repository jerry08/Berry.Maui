using Microsoft.Maui.Controls;

[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.BerryNamespacePrefix + nameof(Berry.Maui.Converters))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.BerryNamespacePrefix + nameof(Berry.Maui.Core))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.BerryNamespacePrefix + nameof(Berry.Maui.Core.Primitives))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.BerryNamespacePrefix + nameof(Berry.Maui.Core.Handlers))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.BerryNamespacePrefix + nameof(Berry.Maui.Core.Views))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.BerryNamespacePrefix + nameof(Berry.Maui.Views))]

[assembly: Microsoft.Maui.Controls.XmlnsPrefix(Constants.XamlNamespace, "berry")]

internal static class Constants
{
    public const string XamlNamespace = "https://github.com/jerry08/Berry.Maui";

    public const string BerryNamespacePrefix = $"{nameof(Berry)}.{nameof(Berry.Maui)}.";
}