using Microsoft.Maui.Graphics;
using AView = Android.Views.View;

namespace Berry.Maui;

internal static class ViewExtensions
{
    internal static Point GetLocationOnScreen(this AView view)
    {
        int[] location = new int[2];
        view.GetLocationOnScreen(location);
        int x = location[0];
        int y = location[1];

        return new Point(x, y);
    }
}
