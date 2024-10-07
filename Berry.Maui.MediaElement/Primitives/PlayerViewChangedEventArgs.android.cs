using System;
using AndroidX.Media3.UI;

namespace Berry.Maui.Primitives;

public sealed class PlayerViewChangedEventArgs(PlayerView playerView) : EventArgs
{
    public PlayerView UpdatedPlayerView { get; } = playerView;
}
