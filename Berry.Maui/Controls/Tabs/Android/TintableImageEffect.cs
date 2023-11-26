using System;
using System.Linq;
using Android.Widget;
using Berry.Maui.Controls.Droid;
using Berry.Maui.Controls.Effects;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;

namespace Berry.Maui.Controls.Droid;

public class AndroidTintableImageEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        UpdateColor();
    }

    protected override void OnDetached() { }

    protected override void OnElementPropertyChanged(
        System.ComponentModel.PropertyChangedEventArgs args
    )
    {
        base.OnElementPropertyChanged(args);

        if ((Element is Image) && args.PropertyName == Image.SourceProperty.PropertyName)
        {
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        var effect = (TintableImageEffect?)
            Element.Effects.FirstOrDefault(x => x is TintableImageEffect);
        var color = effect?.TintColor?.ToAndroid();

        if (Control is ImageView imageView && imageView.Handle != IntPtr.Zero && color.HasValue)
        {
            var tint = color.Value;

            imageView.SetColorFilter(tint);
        }
    }
}
