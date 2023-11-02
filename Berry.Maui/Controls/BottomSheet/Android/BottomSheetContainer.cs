using Android.Content;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;

namespace Berry.Maui.Controls;

internal class BottomSheetContainer : FrameLayout
{
    AView _contentView;
    BottomSheetBackdrop _backdrop;

    public AView ContentView => _contentView;
    public BottomSheetBackdrop Backdrop => _backdrop;

    public BottomSheetContainer(Context context, AView contentView)
        : base(context)
    {
        _contentView = contentView;
        _backdrop = new BottomSheetBackdrop(context);
        AddView(_backdrop);
        AddView(_contentView);
    }

    internal void SetBackdropVisibility(bool hasBackdrop)
    {
        _backdrop.Visibility = hasBackdrop ? ViewStates.Visible : ViewStates.Gone;
    }
}
