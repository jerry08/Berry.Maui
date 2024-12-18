﻿using System;
using System.Collections.Generic;
using UIKit;

namespace Berry.Maui.Behaviors;

internal static class KeyboardModifiersExtensions
{
    internal static UIKeyModifierFlags ToPlatformModifiers(this KeyboardModifiers virtualModifiers)
    {
        List<UIKeyModifierFlags> platformModifierValues = [];

        foreach (KeyboardModifiers virtualModifier in Enum.GetValues(typeof(KeyboardModifiers)))
        {
            if (virtualModifiers.HasFlag(virtualModifier))
            {
                var platformModifier = ToPlatformModifier(virtualModifier);

                if (platformModifier != 0)
                    platformModifierValues.Add(platformModifier);
            }
        }

        var platformModifiers = ToPlatformModifiers(platformModifierValues);

        return platformModifiers;
    }

    internal static KeyboardModifiers ToVirtualModifiers(this UIKeyModifierFlags platformModifiers)
    {
        List<KeyboardModifiers> virtualModifierValues = [];

        foreach (UIKeyModifierFlags platformModifier in Enum.GetValues(typeof(UIKeyModifierFlags)))
        {
            if (platformModifiers.HasFlag(platformModifier))
            {
                var virtualModifier = ToVirtualModifier(platformModifier);

                if (virtualModifier != 0)
                    virtualModifierValues.Add(virtualModifier);
            }
        }

        var virtualModifiers = ToVirtualModifiers(virtualModifierValues);

        return virtualModifiers;
    }

    static UIKeyModifierFlags ToPlatformModifiers(List<UIKeyModifierFlags> platformModifierValues)
    {
        UIKeyModifierFlags platformModifiers = 0;

        foreach (var platformModifier in platformModifierValues)
            platformModifiers |= platformModifier;

        return platformModifiers;
    }

    static KeyboardModifiers ToVirtualModifiers(List<KeyboardModifiers> virtualModifierValues)
    {
        KeyboardModifiers virtualModifiers = 0;

        foreach (var virtualModifer in virtualModifierValues)
            virtualModifiers |= virtualModifer;

        return virtualModifiers;
    }

    static UIKeyModifierFlags ToPlatformModifier(KeyboardModifiers virtualModifier) =>
        virtualModifier switch
        {
            KeyboardModifiers.Alt => UIKeyModifierFlags.Alternate,
            KeyboardModifiers.Command => UIKeyModifierFlags.Command,
            KeyboardModifiers.Control => UIKeyModifierFlags.Control,
            KeyboardModifiers.Shift => UIKeyModifierFlags.Shift,
            _ => 0,
        };

    static KeyboardModifiers ToVirtualModifier(UIKeyModifierFlags platformModifier) =>
        platformModifier switch
        {
            UIKeyModifierFlags.AlphaShift => KeyboardModifiers.Shift,
            UIKeyModifierFlags.Alternate => KeyboardModifiers.Alt,
            UIKeyModifierFlags.Command => KeyboardModifiers.Command,
            UIKeyModifierFlags.Control => KeyboardModifiers.Control,
            UIKeyModifierFlags.Shift => KeyboardModifiers.Shift,
            _ => 0,
        };
}
