using Android.OS;

namespace Berry.Maui.Services;

class BoundServiceBinder(MediaControlsService mediaControlsService) : Binder
{
    public MediaControlsService? Service { get; private set; } = mediaControlsService;
}
