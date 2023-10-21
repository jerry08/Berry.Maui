﻿using Android.Views;
using Java.Lang;

namespace Berry.Maui.Controls;

internal class ViewPropertyAnimatorRunnable : Object, IRunnable
{
    System.Action _action;

    public ViewPropertyAnimatorRunnable(System.Action action)
    {
        _action = action;
    }

    public void Run()
    {
        _action();
    }
}

internal static class ViewPropertyAnimatorExtensions
{
    public static ViewPropertyAnimator WithEndAction(
        this ViewPropertyAnimator animator,
        System.Action endAction
    )
    {
        return animator.WithEndAction(new ViewPropertyAnimatorRunnable(endAction));
    }
}
