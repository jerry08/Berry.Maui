using System;

namespace Berry.Maui.Behaviors;

/// <summary>
/// Provides data for the <see cref="TouchBehavior.TouchGestureCompleted"/> event.
/// </summary>
public class TouchGestureCompletedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TouchGestureCompletedEventArgs"/> class.
    /// </summary>
    internal TouchGestureCompletedEventArgs(object? parameter) => Parameter = parameter;

    /// <summary>
    /// Gets the parameter associated with the touch event.
    /// </summary>
    public object? Parameter { get; }
}
