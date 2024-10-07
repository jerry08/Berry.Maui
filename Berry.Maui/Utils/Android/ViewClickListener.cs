using System;
using Android.Views;

namespace Berry.Maui.Utils;

public class ViewClickListener : Java.Lang.Object, View.IOnClickListener
{
    public Action? ClickAction;

    public ViewClickListener() { }

    public ViewClickListener(Action? action) => ClickAction = action;

    public void OnClick(View? v)
    {
        ClickAction?.Invoke();
    }
}
