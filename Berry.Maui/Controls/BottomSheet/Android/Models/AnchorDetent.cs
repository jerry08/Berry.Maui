using System;
using Microsoft.Maui.Devices;
using AView = Android.Views.View;

namespace Berry.Maui.Controls;

public partial class AnchorDetent : Detent
{
    partial void UpdateHeight(BottomSheet page, double maxSheetHeight)
    {
        if (Anchor == null)
        {
            throw new Exception("Could not update Detent height: Anchor is not set");
        }
        var p = ((AView)Anchor.Handler.PlatformView).GetLocationOnScreen();
        var r = ((AView)page.Handler.PlatformView).GetLocationOnScreen();

        var offset = p - r;

        _height = offset.Height / DeviceDisplay.MainDisplayInfo.Density;
    }
}
