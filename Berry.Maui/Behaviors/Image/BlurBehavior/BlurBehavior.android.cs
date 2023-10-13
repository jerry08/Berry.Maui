using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Berry.Maui.Extensions;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Behaviors;

public partial class BlurBehavior : PlatformBehavior<Image, ImageView>
{
    ImageView? imageView;

    protected override void OnAttachedTo(Image bindable, ImageView platformView)
    {
        imageView = platformView;

        if (OperatingSystem.IsAndroidVersionAtLeast(31))
        {
            SetRendererEffect(platformView, Radius);
            return;
        }

        imageView.LayoutChange += ImageView_LayoutChange;
    }

    private void ImageView_LayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
    {
        if (imageView?.Drawable is not null)
        {
            SetRendererEffect(imageView, 0);
        }
    }

    protected override void OnDetachedFrom(Image bindable, ImageView platformView)
    {
        SetRendererEffect(platformView, 0);
    }

    private void SetRendererEffect(ImageView imageView, float radius)
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(31))
        {
            var renderEffect = radius > 0 ? GetEffect(radius) : null;
            imageView.SetRenderEffect(renderEffect);
        }
        else
        {
            if (
                imageView.Drawable is BitmapDrawable bitmapDrawable
                && bitmapDrawable.Bitmap is not null
            )
            {
                var bitmap = Platform.AppContext.Blur(bitmapDrawable.Bitmap);
                imageView.SetImageBitmap(bitmap);
            }
        }
    }

    private static RenderEffect? GetEffect(float radius)
    {
        return OperatingSystem.IsAndroidVersionAtLeast(31)
            ? RenderEffect.CreateBlurEffect(radius, radius, Shader.TileMode.Decal!)
            : null;
    }
}
