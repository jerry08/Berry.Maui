using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml;

namespace Berry.Maui.Behaviors;

public partial class KeyboardBehavior : PlatformBehavior<VisualElement>
{
    protected override void OnAttachedTo(VisualElement bindable, FrameworkElement platformView)
    {
        base.OnAttachedTo(bindable, platformView);
        platformView.KeyDown += OnKeyDown;
        platformView.KeyUp += OnKeyUp;
        platformView.PreviewKeyDown += OnPreviewKeyDown;
        platformView.PreviewKeyUp += OnPreviewKeyUp;
    }

    protected override void OnDetachedFrom(VisualElement bindable, FrameworkElement platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        platformView.KeyDown -= OnKeyDown;
        platformView.KeyUp -= OnKeyUp;
        platformView.PreviewKeyDown -= OnPreviewKeyDown;
        platformView.PreviewKeyUp -= OnPreviewKeyUp;
    }

    void OnKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e) { }

    void OnPreviewKeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e) { }

    void OnPreviewKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e) { }

    void OnKeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e) { }
}
