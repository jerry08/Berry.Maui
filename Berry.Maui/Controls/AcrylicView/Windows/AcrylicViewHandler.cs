using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Media;
using Border = Microsoft.UI.Xaml.Controls.Border;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace Berry.Maui.Controls;

public partial class AcrylicViewHandler : ViewHandler<IAcrylicView, Border>
{
    private AcrylicBrush _acrylicBrush;

    protected override Border CreatePlatformView()
    {
        _acrylicBrush = new AcrylicBrush();
        return new Border() { Child = new Grid { Background = _acrylicBrush } };
    }

    private static void MapTintColor(AcrylicViewHandler handler, IAcrylicView view)
    {
        if (view.EffectStyle != EffectStyle.Custom)
            return;
        handler._acrylicBrush.TintColor = view.TintColor.ToWindowsColor();
    }

    private static void MapTintOpacity(AcrylicViewHandler handler, IAcrylicView view)
    {
        if (view.EffectStyle != EffectStyle.Custom)
            return;
        handler._acrylicBrush.TintOpacity = view.TintOpacity;
    }

    private static void MapBorderThickness(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
        nativeView.BorderThickness = view.BorderThickness.ToPlatform();
    }

    private static void MapCornerRadius(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
        nativeView.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(
            view.CornerRadius.Left,
            view.CornerRadius.Top,
            view.CornerRadius.Right,
            view.CornerRadius.Bottom
        );
    }

    static void MapContent(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        var grid = nativeView.Child as Grid;
        if (grid is null)
            return;
        grid.Children.Clear();
        if (view.Content is IView content && view.Handler is not null)
        {
            var frameworkElement = content.ToPlatform(view.Handler.MauiContext);
            grid.Children.Add(frameworkElement);
        }
    }

    private static void MapBorderColor(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;

        nativeView.BorderBrush = view.BorderColor.ToPlatform();
    }

    private static void MapEffectStyle(AcrylicViewHandler handler, IAcrylicView view)
    {
        if (view.EffectStyle == EffectStyle.Custom)
        {
            MapTintColor(handler, view);
            MapTintOpacity(handler, view);
            return;
        }

        switch (view.EffectStyle)
        {
            case EffectStyle.Dark:
                handler._acrylicBrush.TintColor = Colors.Black.ToWindowsColor();
                handler._acrylicBrush.TintOpacity = 0.3;
                break;

            case EffectStyle.ExtraDark:
                handler._acrylicBrush.TintColor = Colors.Black.ToWindowsColor();
                handler._acrylicBrush.TintOpacity = 0.6;
                break;

            case EffectStyle.Light:
                handler._acrylicBrush.TintColor = Colors.Transparent.ToWindowsColor();
                handler._acrylicBrush.TintOpacity = 0.0;
                break;

            case EffectStyle.ExtraLight:
                handler._acrylicBrush.TintColor = Colors.White.ToWindowsColor();
                handler._acrylicBrush.TintOpacity = 0.3;
                break;
        }
    }
}
