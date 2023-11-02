using System;
using Google.Android.Material.BottomSheet;
using AView = Android.Views.View;

namespace Berry.Maui.Controls;

public class BottomSheetCallback : BottomSheetBehavior.BottomSheetCallback
{
    readonly BottomSheet _page;

    public event EventHandler StateChanged;

    public BottomSheetCallback(BottomSheet page)
    {
        _page = page;
    }

    public override void OnSlide(AView bottomSheet, float newState) { }

    public override void OnStateChanged(AView view, int newState)
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}
