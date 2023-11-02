using System;
using Android.Content;
using Android.Views;
using Microsoft.Maui.Platform;

namespace Berry.Maui.Behaviors;

public class NotifyingContentViewGroup : ContentViewGroup
{
    public event EventHandler<MotionEvent?>? DispatchTouch;

    public NotifyingContentViewGroup(Context context)
        : base(context)
    {
        SetClipChildren(false);
    }

    public override bool DispatchTouchEvent(MotionEvent? e)
    {
        var result = base.DispatchTouchEvent(e);

        DispatchTouch?.Invoke(this, e);

        return result;
    }
}
