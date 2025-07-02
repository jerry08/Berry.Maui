using AVKit;
using Berry.Maui.Core.Views;
using Berry.Maui.Views;
using Microsoft.Maui.Handlers;

namespace Berry.Maui.Core.Handlers;

public partial class MediaElementHandler : ViewHandler<MediaElement, MauiMediaElement>, IDisposable
{
	AVPlayerViewController? playerViewController;

	/// <inheritdoc/>
	/// <exception cref="NullReferenceException">Thrown if <see cref="MauiContext"/> is <see langword="null"/>.</exception>
	protected override MauiMediaElement CreatePlatformView()
	{
		if (MauiContext is null)
		{
			throw new InvalidOperationException($"{nameof(MauiContext)} cannot be null");
		}

		MediaManager ??= new(MauiContext,
			VirtualView,
			Dispatcher.GetForCurrentThread() ?? throw new InvalidOperationException($"{nameof(IDispatcher)} cannot be null"));


		(_, playerViewController) = MediaManager.CreatePlatformView();

		return new(playerViewController, VirtualView);
	}

	/// <inheritdoc/>
	protected override void DisconnectHandler(MauiMediaElement platformView)
	{
		platformView.Dispose();
		Dispose();

		base.DisconnectHandler(platformView);
	}

	partial void PlatformDispose()
	{
		playerViewController?.Dispose();
		playerViewController = null;
	}
}