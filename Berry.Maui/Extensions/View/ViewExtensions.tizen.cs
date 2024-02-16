using NView = Tizen.NUI.BaseComponents.View;

namespace Berry.Maui.Extensions;

// Copied from https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/Tizen/ViewExtensions.cs
public static partial class ViewExtensions
{
    internal static NView? GetParent(this NView? view)
    {
        return view?.GetParent() as NView;
    }
}
