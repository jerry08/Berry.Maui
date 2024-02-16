using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Berry.Maui.Controls;

public partial class AcrylicViewHandler : ViewHandler<IAcrylicView, BorderView>
{
    // Color layer
    private UIView colorBlendUIView;

    private UIVisualEffectView acrylicEffectView;

    protected override BorderView CreatePlatformView()
    {
        var borderView = new BorderView
        {
            CrossPlatformMeasure = new Func<double, double, Size>(VirtualView.CrossPlatformMeasure),
            CrossPlatformArrange = new Func<Rect, Size>(VirtualView.CrossPlatformArrange)
        };

        colorBlendUIView = [];

        acrylicEffectView = new UIVisualEffectView()
        {
            Frame = VirtualView.Frame,
            ClipsToBounds = true,
            Effect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light)
        };

        return borderView;
    }

    static void MapTintColor(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        handler.colorBlendUIView.BackgroundColor = view.TintColor.ToPlatform();
    }

    static void MapTintOpacity(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        handler.colorBlendUIView.Alpha = (float)view.TintOpacity;
    }

    static void MapBorderColor(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
        nativeView.BorderColor = view.BorderColor.ToCGColor();
    }

    static void MapBorderThickness(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        nativeView.BorderThickness = view.BorderThickness;
    }

    static void MapCornerRadius(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
        nativeView.CornerRadius = view.CornerRadius;
    }

    static void MapContent(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        nativeView.ClearSubviews();

        // Add acrylic layer
        handler.acrylicEffectView.Frame = UIScreen.MainScreen.Bounds;
        nativeView.AddSubview(handler.acrylicEffectView);

        // Add color layer
        handler.colorBlendUIView.Frame = UIScreen.MainScreen.Bounds;
        nativeView.AddSubview(handler.colorBlendUIView);

        // Join Maui view
        if (view.PresentedContent is IView content && view.Handler is not null)
        {
            var frameworkElement = content.ToPlatform(view.Handler.MauiContext);
            nativeView.AddSubview(frameworkElement);
        }
    }

    private static void MapEffectStyle(AcrylicViewHandler? handler, IAcrylicView view)
    {
        if (view.EffectStyle == EffectStyle.Custom)
            return;

        if (handler is null)
            return;

        //var ver = UIDevice.CurrentDevice.SystemVersion;

        var style = view.EffectStyle switch
        {
            EffectStyle.Light => UIBlurEffectStyle.Light,
            EffectStyle.Dark => UIBlurEffectStyle.Dark,
            EffectStyle.ExtraLight => UIBlurEffectStyle.ExtraLight,
            EffectStyle.ExtraDark => UIBlurEffectStyle.Dark,
            _ => UIBlurEffectStyle.Light
        };
        handler.acrylicEffectView.Effect = UIBlurEffect.FromStyle(style);
    }
}
