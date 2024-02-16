using Microsoft.UI.Xaml;

namespace Berry.Maui.Extensions;

// Copied from https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/Windows/ViewExtensions.cs
public static partial class ViewExtensions
{
    internal static DependencyObject? GetParent(this FrameworkElement? view)
    {
        return view?.Parent;
    }

    internal static DependencyObject? GetParent(this DependencyObject? view)
    {
        if (view is FrameworkElement pv)
            return pv.Parent;

        return null;
    }
}
