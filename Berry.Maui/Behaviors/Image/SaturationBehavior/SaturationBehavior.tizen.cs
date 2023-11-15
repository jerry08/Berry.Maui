using Microsoft.Maui.Controls;
using Tizen.NUI.BaseComponents;

namespace Berry.Maui.Behaviors;

public partial class SaturationBehavior : PlatformBehavior<Image, ImageView>
{
    ImageView? imageView;

    protected override void OnAttachedTo(Image bindable, ImageView platformView)
    {
        imageView = platformView;
        SetRendererEffect(platformView, Saturation);
    }

    protected override void OnDetachedFrom(Image bindable, ImageView platformView)
    {
        SetRendererEffect(platformView, 0);
    }

    void SetRendererEffect(ImageView imageView, float radius) { }
}
