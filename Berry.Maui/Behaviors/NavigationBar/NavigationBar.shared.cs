using Microsoft.Maui.Graphics;

namespace Berry.Maui.Behaviors;

/// <summary>
/// Class that hold the method to customize the NavigationBar
/// </summary>
public static partial class NavigationBar
{
    /// <summary>
    /// Method to change the color of the navigation bar.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> that will be set to the navigation bar.</param>
    public static void SetColor(Color? color) => PlatformSetColor(color ?? Colors.Transparent);

    /// <summary>
    /// Method to change the style of the navigation bar.
    /// </summary>
    /// <param name="navigationBarStyle"> The <see cref="NavigationBarStyle"/> that will used by navigation bar.</param>
    public static void SetStyle(NavigationBarStyle navigationBarStyle) =>
        PlatformSetStyle(navigationBarStyle);
}
