using System;
using System.Runtime.CompilerServices;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using PhoenixPortal.Behaviors;

namespace Berry.Maui.Behaviors;

/// <summary>
/// Represents the style of the navigation bar on Android.
/// </summary>
public enum NavigationBarStyle
{
    /// <summary>
    /// The default navigation bar style.
    /// </summary>
    Default = 0,

    /// <summary>
    /// The light content navigation bar style.
    /// </summary>
    LightContent = 1,

    /// <summary>
    /// The dark content navigation bar style.
    /// </summary>
    DarkContent = 2,
}

/// <summary>
/// When to apply the navigation bar color and style.
/// </summary>
public enum NavigationBarApplyOn
{
    /// <summary>
    /// Apply color and style when the behavior has been attached to the page.
    /// </summary>
    OnBehaviorAttachedTo,

    /// <summary>
    /// Apply color and style when the page has been navigated to.
    /// </summary>
    OnPageNavigatedTo,
}

public class NavigationBarBehavior : PlatformBehavior<Page>
{
    /// <summary>
    /// <see cref="BindableProperty"/> that manages the NavigationBarColor property.
    /// </summary>
    public static readonly BindableProperty NavigationBarColorProperty = BindableProperty.Create(
        nameof(NavigationBarColor),
        typeof(Color),
        typeof(NavigationBarBehavior),
        Colors.Transparent
    );

    /// <summary>
    /// <see cref="BindableProperty"/> that manages the NavigationBarColor property.
    /// </summary>
    public static readonly BindableProperty NavigationBarStyleProperty = BindableProperty.Create(
        nameof(NavigationBarStyle),
        typeof(NavigationBarStyle),
        typeof(NavigationBarBehavior),
        NavigationBarStyle.Default
    );

    /// <summary>
    /// <see cref="BindableProperty"/> that manages the ApplyOn property.
    /// </summary>
    public static readonly BindableProperty ApplyOnProperty = BindableProperty.Create(
        nameof(ApplyOn),
        typeof(NavigationBarApplyOn),
        typeof(NavigationBarBehavior),
        NavigationBarApplyOn.OnBehaviorAttachedTo
    );

    /// <summary>
    /// Property that holds the value of the Navigation bar color.
    /// </summary>
    public Color NavigationBarColor
    {
        get => (Color)GetValue(NavigationBarColorProperty);
        set => SetValue(NavigationBarColorProperty, value);
    }

    /// <summary>
    /// Property that holds the value of the Navigation bar color.
    /// </summary>
    public NavigationBarStyle NavigationBarStyle
    {
        get => (NavigationBarStyle)GetValue(NavigationBarStyleProperty);
        set => SetValue(NavigationBarStyleProperty, value);
    }

    /// <summary>
    /// When the navigation bar color and style should be applied.
    /// </summary>
    public NavigationBarApplyOn ApplyOn
    {
        get => (NavigationBarApplyOn)GetValue(ApplyOnProperty);
        set => SetValue(ApplyOnProperty, value);
    }

#if !(WINDOWS || MACCATALYST || TIZEN)

    /// <inheritdoc />
#if IOS
    protected override void OnAttachedTo(Page bindable, UIKit.UIView platformView)
#elif ANDROID
    protected override void OnAttachedTo(Page bindable, Android.Views.View platformView)
#else
    protected override void OnAttachedTo(Page bindable, object platformView)
#endif
    {
        if (ApplyOn == NavigationBarApplyOn.OnBehaviorAttachedTo)
        {
            NavigationBar.SetColor(NavigationBarColor);
            NavigationBar.SetStyle(NavigationBarStyle);
        }

        bindable.NavigatedTo += OnPageNavigatedTo;
#if IOS
        bindable.SizeChanged += OnPageSizeChanged;
#endif
    }

    /// <inheritdoc />
#if IOS
    protected override void OnDetachedFrom(Page bindable, UIKit.UIView platformView)
#elif ANDROID
    protected override void OnDetachedFrom(Page bindable, Android.Views.View platformView)
#else
    protected override void OnDetachedFrom(Page bindable, object platformView)
#endif
    {
#if IOS
        bindable.SizeChanged -= OnPageSizeChanged;
#endif
        bindable.NavigatedTo -= OnPageNavigatedTo;
    }

#if IOS
    void OnPageSizeChanged(object? sender, EventArgs e)
    {
        //NavigationBar.UpdateBarSize();
    }
#endif

    void OnPageNavigatedTo(object? sender, NavigatedToEventArgs e)
    {
        if (ApplyOn == NavigationBarApplyOn.OnPageNavigatedTo)
        {
            NavigationBar.SetColor(NavigationBarColor);
            NavigationBar.SetStyle(NavigationBarStyle);
        }
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return;
        }

#if ANDROID
        if (Platform.CurrentActivity is null)
        {
            return;
        }
#endif

        if (
            propertyName.IsOneOf(
                NavigationBarColorProperty,
                VisualElement.WidthProperty,
                VisualElement.HeightProperty
            )
        )
        {
            NavigationBar.SetColor(NavigationBarColor);
        }
        else if (propertyName == NavigationBarStyleProperty.PropertyName)
        {
            NavigationBar.SetStyle(NavigationBarStyle);
        }
    }
#endif
}
