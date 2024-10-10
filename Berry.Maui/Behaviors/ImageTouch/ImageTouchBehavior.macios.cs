using System;
using Microsoft.Maui.Controls;
using UIKit;
using IImage = Microsoft.Maui.IImage;

namespace Berry.Maui.Behaviors;

public partial class ImageTouchBehavior
{
    /// <inheritdoc/>
    protected override void OnAttachedTo(VisualElement bindable, UIView platformView)
    {
        if (bindable is not IImage)
        {
            throw new InvalidOperationException(
                $"{nameof(ImageTouchBehavior)} can only be attached to an {nameof(IImage)}"
            );
        }

        base.OnAttachedTo(bindable, platformView);
    }
}
