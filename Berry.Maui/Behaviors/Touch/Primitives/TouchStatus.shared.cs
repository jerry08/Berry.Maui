﻿namespace Berry.Maui.Behaviors;

/// <summary>
/// Provides data for the <see cref="TouchBehavior.CurrentTouchStatusChanged"/> event.
/// </summary>
public enum TouchStatus
{
    /// <summary>
    /// The touch interaction has started.
    /// </summary>
    Started,

    /// <summary>
    /// The touch interaction has completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The touch interaction has been canceled.
    /// </summary>
    Canceled,
}
