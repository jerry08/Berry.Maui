using System;

namespace Berry.Maui.Behaviors;

/// <summary>
/// Provides data for the <see cref="TouchBehavior.CurrentTouchStateChanged"/> event.
/// </summary>
public class TouchStateChangedEventArgs : EventArgs
{
    internal TouchStateChangedEventArgs(TouchState state) => State = state;

    /// <summary>
    /// Gets the current state of the touch event.
    /// </summary>
    public TouchState State { get; }
}
