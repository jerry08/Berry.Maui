using System;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Behaviors;

public partial class KeyboardBehavior : PlatformBehavior<VisualElement>
{
    KeyboardBehaviorTriggers? _triggers;

    public KeyboardBehaviorTriggers Triggers => _triggers ??= [];

    public event EventHandler<KeyPressedEventArgs>? KeyDown;
    public event EventHandler<KeyPressedEventArgs>? KeyUp;

    internal void RaiseKeyDown(KeyPressedEventArgs args) => KeyDown?.Invoke(this, args);

    internal void RaiseKeyUp(KeyPressedEventArgs args) => KeyUp?.Invoke(this, args);
}
