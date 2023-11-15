using Microsoft.Maui.Handlers;
using Tizen.NUI.BaseComponents;

namespace Berry.Maui.Controls;

public partial class AcrylicViewHandler : ViewHandler<IAcrylicView, View>
{
    protected override View CreatePlatformView()
    {
        throw new System.NotImplementedException();
    }

    static void MapTintColor(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
    }

    static void MapTintOpacity(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
    }

    static void MapBorderColor(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
    }

    static void MapBorderThickness(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
    }

    static void MapCornerRadius(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
    }

    static void MapContent(AcrylicViewHandler handler, IAcrylicView view)
    {
        var nativeView = handler?.PlatformView;
        if (nativeView is null)
            return;
    }

    private static void MapEffectStyle(AcrylicViewHandler handler, IAcrylicView view)
    {
        if (view.EffectStyle == EffectStyle.Custom)
            return;
        if (handler is null)
            return;
    }
}
