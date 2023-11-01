using Android.Graphics.Drawables;

namespace Berry.Maui;

internal class SheetRadiusDrawable: GradientDrawable
{
    public SheetRadiusDrawable()
    {
        
    }

    internal void SetCornerRadius(int radius)
    {
        SetCornerRadii(new float[] { radius, radius, radius, radius, 0, 0, 0, 0 });
    }
}
