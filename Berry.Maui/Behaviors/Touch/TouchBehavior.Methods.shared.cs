﻿using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Behaviors;

public partial class TouchBehavior : IDisposable
{
    readonly NullReferenceException nre = new(nameof(Element));

    internal void RaiseInteractionStatusChanged() =>
        weakEventManager.HandleEvent(
            Element ?? throw nre,
            new TouchInteractionStatusChangedEventArgs(InteractionStatus),
            nameof(InteractionStatusChanged)
        );

    internal void RaiseStatusChanged() =>
        weakEventManager.HandleEvent(
            Element ?? throw nre,
            new TouchStatusChangedEventArgs(Status),
            nameof(StatusChanged)
        );

    internal void RaiseHoverStateChanged()
    {
        ForceUpdateState();
        weakEventManager.HandleEvent(
            Element ?? throw nre,
            new HoverStateChangedEventArgs(HoverState),
            nameof(HoverStateChanged)
        );
    }

    internal void RaiseHoverStatusChanged() =>
        weakEventManager.HandleEvent(
            Element ?? throw nre,
            new HoverStatusChangedEventArgs(HoverStatus),
            nameof(HoverStatusChanged)
        );

    internal void RaiseTouchGestureCompleted()
    {
        var element = Element;
        if (element is null)
        {
            return;
        }

        var parameter = CommandParameter;

        if (Command?.CanExecute(parameter) is true)
        {
            Command.Execute(parameter);
        }

        weakEventManager.HandleEvent(
            element,
            new TouchGestureCompletedEventArgs(parameter),
            nameof(TouchGestureCompleted)
        );
    }

    internal void RaiseLongPressCompleted()
    {
        var element = Element;
        if (element is null)
        {
            return;
        }

        var parameter = LongPressCommandParameter ?? CommandParameter;
        LongPressCommand?.Execute(parameter);
        weakEventManager.HandleEvent(
            element,
            new LongPressCompletedEventArgs(parameter),
            nameof(LongPressCompleted)
        );
    }

    internal void ForceUpdateState(bool animated = true)
    {
        if (element is null)
        {
            return;
        }

        gestureManager
            .ChangeStateAsync(this, animated)
            .ContinueWith(
                t =>
                {
                    if (t.Exception is null)
                    {
                        return;
                    }

                    Console.WriteLine(
                        $"Failed to force update state, with the {t.Exception} exception and the {t.Exception.Message} message."
                    );
                },
                TaskContinuationOptions.OnlyOnFaulted
            );
    }

    internal void HandleTouch(TouchStatus status) => gestureManager.HandleTouch(this, status);

    internal void HandleUserInteraction(TouchInteractionStatus interactionStatus) =>
        gestureManager.HandleUserInteraction(this, interactionStatus);

    internal void HandleHover(HoverStatus status) => gestureManager.HandleHover(this, status);

    internal void RaiseStateChanged()
    {
        ForceUpdateState();
        HandleLongPress();
        weakEventManager.HandleEvent(
            Element ?? throw nre,
            new TouchStateChangedEventArgs(State),
            nameof(StateChanged)
        );
    }

    internal void HandleLongPress()
    {
        if (Element is null)
        {
            return;
        }

        gestureManager.HandleLongPress(this);
    }

    void SetChildrenInputTransparent(bool value)
    {
        if (Element is not Layout layout)
        {
            return;
        }

        layout.ChildAdded -= OnLayoutChildAdded;

        if (!value)
        {
            return;
        }

        layout.InputTransparent = false;
        foreach (var view in layout.Children)
        {
            OnLayoutChildAdded(layout, new ElementEventArgs((View)view));
        }

        layout.ChildAdded += OnLayoutChildAdded;
    }

    void OnLayoutChildAdded(object? sender, ElementEventArgs e)
    {
        if (e.Element is not View view)
        {
            return;
        }

        if (!ShouldMakeChildrenInputTransparent)
        {
            view.InputTransparent = false;
            return;
        }

        view.InputTransparent = IsEnabled;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    bool isDisposed;

    /// <summary>
    /// Dispose the object.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing)
        {
            // free managed resources
            gestureManager.Dispose();
        }

        isDisposed = true;
    }
}
