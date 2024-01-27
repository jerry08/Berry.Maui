using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Berry.Maui.Handlers;

public partial class DatePickerViewHandler : DatePickerHandler
{
    protected override MauiDatePicker CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();

        using (var gradientDrawable = new GradientDrawable())
        {
            gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
            nativeView.SetBackground(gradientDrawable);
            nativeView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(
                Colors.Transparent.ToAndroid()
            );
        }

        return nativeView;
    }
}
