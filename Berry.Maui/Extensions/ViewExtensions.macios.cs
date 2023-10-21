using UIKit;

namespace Berry.Maui.Extensions;

// Copied from https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/iOS/ViewExtensions.cs
public static partial class ViewExtensions
{
    internal static UIView? GetParent(this UIView? view)
    {
        return view?.Superview;
    }
}
