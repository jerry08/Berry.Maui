using Microsoft.Maui;
using Microsoft.Maui.Devices;

namespace Berry.Maui;

// Copied from https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Application/Application.cs
public static partial class ApplicationEx
{
    public static partial void SetOrientation(DisplayOrientation orientation);

    public static bool IsApplicationOrNull(object? element) => element is null or IApplication;

    public static bool IsApplicationOrWindowOrNull(object? element) =>
        element is null or IApplication or IWindow;
}

public static partial class ApplicationEx
{
#if !(ANDROID || MACCATALYST || IOS)
    public static int GetStatusBarHeight() => 0;

    public static int GetNavigationBarHeight() => 0;
#endif
}
