namespace Berry.Maui.Behaviors;

/// <summary>
/// Provides data for the <see cref="TouchBehavior.CurrentTouchStatusChanged"/> event.
/// </summary>
public enum TouchState
{
    /// <summary>
    /// The pointer is not over the element.
    /// </summary>
    Default,

    /// <summary>
    /// The pointer is over the element.
    /// </summary>
    Pressed
}
