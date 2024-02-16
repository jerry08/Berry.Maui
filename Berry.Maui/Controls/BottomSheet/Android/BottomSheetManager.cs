using Microsoft.Maui.Controls;

namespace Berry.Maui.Controls;

internal partial class BottomSheetManager
{
    static partial void PlatformShow(Window window, BottomSheet sheet, bool animated)
    {
        var controller = new BottomSheetController(window.Handler.MauiContext, sheet);
        sheet.Controller = controller;
        controller.Show(animated);
    }
}
