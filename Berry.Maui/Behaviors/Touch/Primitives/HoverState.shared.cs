﻿namespace Berry.Maui.Behaviors;

/// <summary>
/// Provides data for the <see cref="TouchBehavior.HoverStateChanged"/> event.
/// </summary>
///
public enum HoverState
{
    /// <summary>
    /// The pointer is not over the element.
    /// </summary>
    Default,

    /// <summary>
    /// The pointer is over the element.
    /// </summary>
    Hovered,
}
