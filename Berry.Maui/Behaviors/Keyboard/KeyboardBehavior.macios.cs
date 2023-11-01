using Microsoft.Maui;
using Microsoft.Maui.Controls;
using UIKit;

namespace Berry.Maui.Behaviors;

public partial class KeyboardBehavior : PlatformBehavior<VisualElement>
{
    protected override void OnAttachedTo(VisualElement bindable, UIView platformView)
    {
        base.OnAttachedTo(bindable, platformView);

        var page = GetParentPage(bindable);

        if (page == null)
            return;

        // Register to key press events
        if (
            page.Handler is not IPlatformViewHandler viewHandler
            || viewHandler.ViewController
                is not KeyboardPageViewController keyboardPageViewController
        )
        {
            return;
        }

        keyboardPageViewController.RegisterKeyboardBehavior(this);
    }

    protected override void OnDetachedFrom(VisualElement bindable, UIView platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        var page = GetParentPage(bindable);

        if (page == null)
            return;

        // Unregister from key press events
        if (
            page.Handler is not IPlatformViewHandler viewHandler
            || viewHandler.ViewController
                is not KeyboardPageViewController keyboardPageViewController
        )
        {
            return;
        }

        keyboardPageViewController.UnregisterKeyboardBehavior(this);
    }

    static Page? GetParentPage(VisualElement element)
    {
        if (element is Page)
            return element as Page;

        var currentElement = (Element?)element;

        while (currentElement is not null and not Page)
            currentElement = currentElement.Parent;

        return currentElement as Page;
    }
}
