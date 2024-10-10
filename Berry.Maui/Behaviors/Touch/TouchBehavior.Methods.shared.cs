using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Behaviors;

public partial class TouchBehavior : IDisposable
{
    internal void RaiseInteractionStatusChanged() =>
        weakEventManager.HandleEvent(
            this,
            new TouchInteractionStatusChangedEventArgs(CurrentInteractionStatus),
            nameof(InteractionStatusChanged)
        );

    internal void RaiseCurrentTouchStatusChanged() =>
        weakEventManager.HandleEvent(
            this,
            new TouchStatusChangedEventArgs(CurrentTouchStatus),
            nameof(CurrentTouchStatusChanged)
        );

    async Task RaiseHoverStateChanged(CancellationToken token)
    {
        await ForceUpdateState(token);
        weakEventManager.HandleEvent(
            this,
            new HoverStateChangedEventArgs(CurrentHoverState),
            nameof(HoverStateChanged)
        );
    }

    internal void RaiseHoverStatusChanged() =>
        weakEventManager.HandleEvent(
            this,
            new HoverStatusChangedEventArgs(CurrentHoverStatus),
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

    internal async Task ForceUpdateState(CancellationToken token, bool animated = true)
    {
        if (Element is null)
        {
            return;
        }

        try
        {
            await gestureManager.ChangeStateAsync(this, animated, token);
        }
        catch (TaskCanceledException ex)
        {
            Trace.TraceInformation("{0}", ex);
        }
    }

    internal void HandleTouch(TouchStatus status) => gestureManager.HandleTouch(this, status);

    internal void HandleUserInteraction(TouchInteractionStatus interactionStatus) =>
        gestureManager.HandleUserInteraction(this, interactionStatus);

    internal void HandleHover(HoverStatus status)
    {
        ObjectDisposedException.ThrowIf(isDisposed, this);

        GestureManager.HandleHover(this, status);
    }

    async Task RaiseCurrentTouchStateChanged(CancellationToken token)
    {
        await Task.WhenAll(ForceUpdateState(token), HandleLongPress(token));
        weakEventManager.HandleEvent(
            this,
            new TouchStateChangedEventArgs(CurrentTouchState),
            nameof(CurrentTouchStateChanged)
        );
    }

    async Task HandleLongPress(CancellationToken token)
    {
        if (Element is null)
        {
            return;
        }

        await gestureManager.HandleLongPress(this, token);
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
