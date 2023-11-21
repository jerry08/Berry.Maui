using Microsoft.Maui.Controls;

namespace Berry.Maui.Behaviors;

// Taken from https://stackoverflow.com/questions/76210918/style-target-behaviors-maui
public class StyleBehavior : Behavior<VisualElement>
{
    public static readonly BindableProperty AttachBehaviorProperty =
        BindableProperty.CreateAttached(
            "AttachBehavior",
            typeof(object),
            typeof(StyleBehavior),
            null,
            propertyChanged: OnAttachBehaviorChanged
        );

    public static object GetAttachBehavior(BindableObject view)
    {
        return view.GetValue(AttachBehaviorProperty);
    }

    public static void SetAttachBehavior(BindableObject view, object value)
    {
        view.SetValue(AttachBehaviorProperty, value);
    }

    static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
    {
        if (view is not VisualElement visualElement)
        {
            return;
        }

        if (newValue is BehaviorsCollection attachBehaviors)
        {
            foreach (var attachBehavior in attachBehaviors)
            {
                visualElement.Behaviors.Add(attachBehavior);
            }
        }
        else if (newValue is Behavior attachBehavior)
        {
            visualElement.Behaviors.Add(attachBehavior);
        }
    }
}
