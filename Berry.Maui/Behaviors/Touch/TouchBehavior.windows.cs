﻿using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;

namespace Berry.Maui.Behaviors;

public partial class TouchBehavior
{
    const string pointerDownAnimationKey = "PointerDownAnimation";

    const string pointerUpAnimationKey = "PointerUpAnimation";

    bool isPressed;

    bool isIntentionalCaptureLoss;

    Storyboard? pointerDownStoryboard;

    Storyboard? pointerUpStoryboard;

    /// <inheritdoc/>
    protected override void OnAttachedTo(VisualElement bindable, FrameworkElement platformView)
    {
        base.OnAttachedTo(bindable, platformView);

        Element = bindable;

        if (string.IsNullOrEmpty(platformView.Name))
        {
            platformView.Name = Guid.NewGuid().ToString();
        }

        if (platformView.Resources.ContainsKey(pointerDownAnimationKey))
        {
            pointerDownStoryboard = (Storyboard)platformView.Resources[pointerDownAnimationKey];
        }
        else
        {
            pointerDownStoryboard = new();
            var downThemeAnimation = new PointerDownThemeAnimation();

            Storyboard.SetTargetName(downThemeAnimation, platformView.Name);
            pointerDownStoryboard.Children.Add(downThemeAnimation);
            platformView.Resources.Add(
                new KeyValuePair<object, object>(pointerDownAnimationKey, pointerDownStoryboard)
            );
        }

        if (platformView.Resources.ContainsKey(pointerUpAnimationKey))
        {
            pointerUpStoryboard = (Storyboard)platformView.Resources[pointerUpAnimationKey];
        }
        else
        {
            pointerUpStoryboard = new();
            var upThemeAnimation = new PointerUpThemeAnimation();

            Storyboard.SetTargetName(upThemeAnimation, platformView.Name);

            pointerUpStoryboard.Children.Add(upThemeAnimation);

            platformView.Resources.Add(
                new KeyValuePair<object, object>(pointerUpAnimationKey, pointerUpStoryboard)
            );
        }

        platformView.PointerPressed += OnPointerPressed;
        platformView.PointerReleased += OnPointerReleased;
        platformView.PointerCanceled += OnPointerCanceled;
        platformView.PointerExited += OnPointerExited;
        platformView.PointerEntered += OnPointerEntered;
        platformView.PointerCaptureLost += OnPointerCaptureLost;
    }

    void AnimateTilt(Storyboard? storyboard)
    {
        if (storyboard is not null && Element is not null && NativeAnimation)
        {
            try
            {
                storyboard.Stop();
                storyboard.Begin();
            }
            catch
            {
                // Suppress
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnDetachedFrom(VisualElement bindable, FrameworkElement platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        if (!IsEnabled)
        {
            return;
        }

        Element = null;

        platformView.PointerPressed -= OnPointerPressed;
        platformView.PointerReleased -= OnPointerReleased;
        platformView.PointerCanceled -= OnPointerCanceled;
        platformView.PointerExited -= OnPointerExited;
        platformView.PointerEntered -= OnPointerEntered;
        platformView.PointerCaptureLost -= OnPointerCaptureLost;

        isPressed = false;
    }

    void OnPointerEntered(object? sender, PointerRoutedEventArgs e)
    {
        if (Element is null || !IsEnabled)
        {
            return;
        }

        HandleHover(HoverStatus.Entered);

        if (isPressed)
        {
            HandleTouch(TouchStatus.Started);
            AnimateTilt(pointerDownStoryboard);
        }
    }

    void OnPointerExited(object? sender, PointerRoutedEventArgs e)
    {
        if (Element is null || !IsEnabled)
        {
            return;
        }

        if (isPressed)
        {
            HandleTouch(TouchStatus.Canceled);
            AnimateTilt(pointerUpStoryboard);
        }

        HandleHover(HoverStatus.Exited);
    }

    void OnPointerCanceled(object? sender, PointerRoutedEventArgs e)
    {
        if (Element is null || !IsEnabled)
        {
            return;
        }

        isPressed = false;

        HandleTouch(TouchStatus.Canceled);
        HandleUserInteraction(TouchInteractionStatus.Completed);
        HandleHover(HoverStatus.Exited);

        AnimateTilt(pointerUpStoryboard);
    }

    void OnPointerCaptureLost(object? sender, PointerRoutedEventArgs e)
    {
        if (Element is null || !IsEnabled)
        {
            return;
        }

        if (isIntentionalCaptureLoss)
        {
            return;
        }

        isPressed = false;

        if (CurrentTouchStatus is not TouchStatus.Canceled)
        {
            HandleTouch(TouchStatus.Canceled);
        }

        HandleUserInteraction(TouchInteractionStatus.Completed);

        if (CurrentHoverStatus is not HoverStatus.Exited)
        {
            HandleHover(HoverStatus.Exited);
        }

        AnimateTilt(pointerUpStoryboard);
    }

    void OnPointerReleased(object? sender, PointerRoutedEventArgs e)
    {
        if (Element is null || !IsEnabled)
        {
            return;
        }

        if (isPressed && (CurrentHoverStatus is HoverStatus.Entered))
        {
            HandleTouch(TouchStatus.Completed);
            AnimateTilt(pointerUpStoryboard);
        }
        else if (CurrentHoverStatus is not HoverStatus.Exited)
        {
            HandleTouch(TouchStatus.Canceled);
            AnimateTilt(pointerUpStoryboard);
        }

        HandleUserInteraction(TouchInteractionStatus.Completed);
        e.Handled = true;

        isPressed = false;
        isIntentionalCaptureLoss = true;
    }

    void OnPointerPressed(object? sender, PointerRoutedEventArgs e)
    {
        if (Element is null || !IsEnabled || sender is not FrameworkElement container)
        {
            return;
        }

        isPressed = true;
        container.CapturePointer(e.Pointer);

        HandleUserInteraction(TouchInteractionStatus.Started);
        HandleTouch(TouchStatus.Started);

        AnimateTilt(pointerDownStoryboard);

        isIntentionalCaptureLoss = false;
        e.Handled = true;
    }
}
