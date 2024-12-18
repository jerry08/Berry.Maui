﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Behaviors;

/// <summary>
/// A common interface to be used across <see cref="BaseBehavior{TView}"/> and <see cref="BasePlatformBehavior{TView,TPlatformView}"/>
/// </summary>
public interface IBerryBehavior<TView>
    where TView : Element
{
    /// <summary>
    /// View used by the Behavior
    /// </summary>
    protected TView? View { get; set; }

    internal bool TrySetBindingContextToAttachedViewBindingContext()
    {
        if (this is not Behavior behavior)
        {
            throw new InvalidOperationException(
                $"{nameof(IBerryBehavior<TView>)} can only be used for a {nameof(Behavior)}"
            );
        }

        //if (behavior.IsSet(BindableObject.BindingContextProperty) || View is null)
        if (View is null)
        {
            return false;
        }

        behavior.SetBinding(
            BindableObject.BindingContextProperty,
            new Binding { Source = View, Path = BindableObject.BindingContextProperty.PropertyName }
        );

        return true;
    }

    internal bool TryRemoveBindingContext()
    {
        if (this is not Behavior behavior)
        {
            throw new InvalidOperationException(
                $"{nameof(IBerryBehavior<TView>)} can only be used for a {nameof(Behavior)}"
            );
        }

        if (behavior.IsSet(BindableObject.BindingContextProperty))
        {
            behavior.RemoveBinding(BindableObject.BindingContextProperty);
            //behavior.ClearValue(BindableObject.BindingContextProperty);
            return true;
        }

        return false;
    }

    [MemberNotNull(nameof(View))]
    internal void AssignViewAndBingingContext(TView bindable)
    {
        View = bindable;
        bindable.PropertyChanged += OnViewPropertyChanged;

        TrySetBindingContextToAttachedViewBindingContext();
    }

    internal void UnassignViewAndBingingContext(TView bindable)
    {
        TryRemoveBindingContext();

        bindable.PropertyChanged -= OnViewPropertyChanged;

        View = null;
    }

    /// <summary>
    /// Executes when <see cref="BindableObject.OnPropertyChanged"/> fires
    /// </summary>
    /// <param name="sender"><see cref="Behavior"/></param>
    /// <param name="e"><see cref="PropertyChangedEventArgs"/> </param>
    /// <exception cref="ArgumentException">Throws when <paramref name="sender"/> is not of type <typeparamref name="TView"/></exception>
    protected void OnViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TView view)
        {
            throw new ArgumentException($"Behavior can only be attached to {typeof(TView)}");
        }

        try
        {
            OnViewPropertyChanged(view, e);
        }
        catch (Exception ex) when (Options.ShouldSuppressExceptionsInBehaviors)
        {
            Trace.WriteLine(ex);
        }
    }

    /// <summary>
    /// Virtual method that executes when a property on the View has changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnViewPropertyChanged(TView sender, PropertyChangedEventArgs e);
}
