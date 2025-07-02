using Android.OS;
using Berry.Maui.Media.Services;

namespace Berry.Maui.Services;

sealed class BoundServiceBinder(MediaControlsService mediaControlsService) : Binder
{
	public MediaControlsService Service { get; } = mediaControlsService;
}