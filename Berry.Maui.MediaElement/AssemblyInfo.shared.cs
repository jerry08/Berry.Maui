[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.CommunityToolkitNamespacePrefix + nameof(Berry.Maui.Converters))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.CommunityToolkitNamespacePrefix + nameof(Berry.Maui.Core))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.CommunityToolkitNamespacePrefix + nameof(Berry.Maui.Core.Handlers))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.CommunityToolkitNamespacePrefix + nameof(Berry.Maui.Core.Views))]
[assembly: XmlnsDefinition(Constants.XamlNamespace, Constants.CommunityToolkitNamespacePrefix + nameof(Berry.Maui.Views))]

[assembly: Microsoft.Maui.Controls.XmlnsPrefix(Constants.XamlNamespace, "toolkit")]

class Constants
{
    public const string XamlNamespace = "https://github.com/jerry08/Berry.Maui";

    public const string CommunityToolkitNamespacePrefix = $"{nameof(Berry)}.{nameof(Berry.Maui)}.";
}