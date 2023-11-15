using Microsoft.Maui.Controls;
using Tizen.NUI.BaseComponents;

namespace Berry.Maui.Behaviors;

public partial class ChainBehavior : PlatformBehavior<Image, ImageView>
{
    ImageView? imageView;

    protected override void OnAttachedTo(Image bindable, ImageView platformView)
    {
        imageView = platformView;
        SetRendererEffect(platformView, Effects);
    }

    protected override void OnDetachedFrom(Image bindable, ImageView platformView)
    {
        SetRendererEffect(platformView, null);
    }

    void SetRendererEffect(ImageView imageView, string? effects) { }
}
