using System;
using Berry.Maui.Core.Views;
using Berry.Maui.Views;
using Microsoft.Maui;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Handlers;

namespace Berry.Maui.Core.Handlers;

public partial class MediaElementHandler : ViewHandler<MediaElement, MauiMediaElement>
{
    /// <inheritdoc/>
    /// <exception cref="NullReferenceException">Thrown if <see cref="MauiContext"/> is <see langword="null"/>.</exception>
    protected override MauiMediaElement CreatePlatformView()
    {
        mediaManager ??= new(
            MauiContext ?? throw new NullReferenceException(),
            VirtualView,
            Dispatcher.GetForCurrentThread()
                ?? throw new InvalidOperationException($"{nameof(IDispatcher)} cannot be null")
        );

        var playerView = mediaManager.CreatePlatformView();
        return new(playerView);
    }

    /// <inheritdoc/>
    protected override void DisconnectHandler(MauiMediaElement platformView)
    {
        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }

    /// <inheritdoc/>
    public static void ShouldLoopPlayback(MediaElementHandler handler, MediaElement MediaElement)
    {
        handler.mediaManager?.UpdateShouldLoopPlayback();
    }
}
