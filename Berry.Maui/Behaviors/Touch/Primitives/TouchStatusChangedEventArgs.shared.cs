using System;

namespace Berry.Maui.Behaviors;

/// <summary>
/// Provides data for the <see cref="TouchBehavior.CurrentTouchStatusChanged"/> event.
/// </summary>
public class TouchStatusChangedEventArgs : EventArgs
{
    internal TouchStatusChangedEventArgs(TouchStatus status) => Status = status;

    /// <summary>
    /// Gets the current touch status.
    /// </summary>
    public TouchStatus Status { get; }
}
