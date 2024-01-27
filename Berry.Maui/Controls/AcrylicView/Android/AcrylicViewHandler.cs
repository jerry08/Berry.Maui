using System;
using Android.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using View = Android.Views.View;

namespace Berry.Maui.Controls;

public partial class AcrylicViewHandler : ViewHandler<IAcrylicView, FrameLayout>
{
    /// <summary>
    /// AcrylicBackground layer
    /// </summary>
    private RealtimeBlurView realtimeBlurView;

    private BorderDrawable colorGradientDrawable;

    private View colorBlendLayer;

    private float colorBlendLayerAlpha;

    private BorderViewGroup borderViewGroup;

    protected override FrameLayout CreatePlatformView()
    {
        colorBlendLayer = new View(Context);

        realtimeBlurView = new RealtimeBlurView(Context, SetContentVisible);
        realtimeBlurView.SetBlurRadius(120);
        realtimeBlurView.SetOverlayColor(Colors.Transparent.ToAndroid());
        realtimeBlurView.SetDownsampleFactor(4);

        borderViewGroup = new BorderViewGroup(Context)
        {
            CrossPlatformMeasure = new Func<double, double, Size>(VirtualView.CrossPlatformMeasure),
            CrossPlatformArrange = new Func<Rect, Size>(VirtualView.CrossPlatformArrange)
        };

        var frame = new FrameLayout(Context);
        frame.AddView(realtimeBlurView);
        frame.AddView(colorBlendLayer);
        frame.AddView(borderViewGroup);

        return frame;
    }

    /// <summary>
    /// Controls the transparency of the top-level view when obtaining the view layer
    /// </summary>
    /// <param name="isVisible"></param>
    private void SetContentVisible(bool isVisible)
    {
        if (borderViewGroup is null)
            return;

        borderViewGroup.Alpha = isVisible ? 1f : 0f;
    }

    private static void MapTintColor(AcrylicViewHandler? handler, IAcrylicView view)
    {
        if (view.EffectStyle != EffectStyle.Custom)
            return;

        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        handler?.UpdateColorblendLayer(view);
    }

    private static void MapTintOpacity(AcrylicViewHandler handler, IAcrylicView view)
    {
        if (view.EffectStyle != EffectStyle.Custom)
            return;

        if (view.TintColor is null || view.TintColor == Colors.Transparent)
            return;

        handler.colorBlendLayerAlpha = (float)view.TintOpacity;
        handler.colorBlendLayer.Alpha = handler.colorBlendLayerAlpha;
    }

    private static void MapEffectStyle(AcrylicViewHandler handler, IAcrylicView view)
    {
        switch (view.EffectStyle)
        {
            case EffectStyle.Dark:
                handler.UpdateEffectStyle(view, Colors.Black, 0.15f);
                break;

            case EffectStyle.ExtraDark:
                handler.UpdateEffectStyle(view, Colors.Black, 0.3f);
                break;

            case EffectStyle.Light:
                handler.UpdateEffectStyle(view, Colors.White, 0.05f);
                break;

            case EffectStyle.ExtraLight:
                handler.UpdateEffectStyle(view, Colors.White, 0.3f);
                break;

            case EffectStyle.Custom:
                handler.UpdateColorblendLayer(view);
                break;
        }
    }

    private void UpdateEffectStyle(IAcrylicView view, Color color, float tintOpacity)
    {
        colorGradientDrawable = new BorderDrawable(Context, view.CornerRadius, color.ToPlatform());
        colorBlendLayer.SetBackgroundDrawable(colorGradientDrawable);

        colorBlendLayerAlpha = tintOpacity;
        colorBlendLayer.Alpha = colorBlendLayerAlpha;
    }

    private static void MapContent(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        handler.borderViewGroup.RemoveAllViews();
        if (view.Content is IView content && view.Handler is not null)
        {
            var view3 = content.ToPlatform(view.Handler.MauiContext);
            handler.borderViewGroup.AddView(view3);
        }
    }

    private static void MapBorderThickness(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
        handler.realtimeBlurView.SetBorderThickness(view.BorderThickness);
        PropertyChanged(handler, view);
    }

    private static void MapCornerRadius(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        handler!.UpdateColorblendLayer(view);

        var thickness = nativeView.Context.ToPixels(view.CornerRadius);

        // Acrylic layer rounded corners
        handler.realtimeBlurView.SetCornerRadius(
            (float)thickness.Left,
            (float)thickness.Top,
            (float)thickness.Right,
            (float)thickness.Bottom
        );

        // Border layer
        PropertyChanged(handler, view);
    }

    private static void MapBorderColor(AcrylicViewHandler handler, IAcrylicView view)
    {
        PropertyChanged(handler, view);
    }

    private static void PropertyChanged(AcrylicViewHandler? handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        handler!.borderViewGroup.BorderDrawable = new BorderDrawable(nativeView.Context, view);
    }

    private void UpdateColorblendLayer(IAcrylicView view)
    {
        if (
            (view.TintColor is null || view.TintColor == Colors.Transparent)
            && view.EffectStyle == EffectStyle.Custom
        )
        {
            colorBlendLayer.SetBackgroundDrawable(null);
        }
        else
        {
            // Mixed color layer fillets
            colorGradientDrawable = new BorderDrawable(
                Context,
                view.CornerRadius,
                view.TintColor.ToPlatform()
            );
            colorBlendLayer.SetBackgroundDrawable(colorGradientDrawable);

            // Set color layer opacity
            colorBlendLayerAlpha = (float)view.TintOpacity;
            colorBlendLayer.Alpha = colorBlendLayerAlpha;
        }
    }
}
