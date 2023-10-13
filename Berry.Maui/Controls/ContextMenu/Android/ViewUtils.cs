using System;
using Android.Content.Res;

namespace Berry.Maui.Controls;

internal class ViewUtils
{
    public static int DpToPx(int dp)
    {
        return (int)Math.Round(dp * Resources.System.DisplayMetrics.Density);
    }
}
