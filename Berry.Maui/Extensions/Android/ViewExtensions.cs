using Android.Views;

namespace Berry.Maui.Extensions;

// Copied from https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/Android/ViewExtensions.cs
public static partial class ViewExtensions
{
    internal static IViewParent? GetParent(this View? view)
    {
        return view?.Parent;
    }

    internal static IViewParent? GetParent(this IViewParent? view)
    {
        return view?.Parent;
    }
}
