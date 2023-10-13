using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace Berry.Maui.Controls;

public class BorderViewGroup : FrameLayout
{
    /// <summary>
    /// Background color, rounded corners, artboard
    /// </summary>
    private BorderDrawable borderDrawable;

    public BorderViewGroup(Context context)
        : base(context) { }

    internal Func<double, double, Size> CrossPlatformMeasure { get; set; }

    internal Func<Rect, Size> CrossPlatformArrange { get; set; }

    public BorderDrawable BorderDrawable
    {
        get { return borderDrawable; }
        set
        {
            if (borderDrawable != value)
            {
                borderDrawable = value;
                Background = borderDrawable;
            }
        }
    }

    protected override void DispatchDraw(Canvas canvas)
    {
        if (borderDrawable is not null)
        {
            canvas.ClipPath(borderDrawable.GetClipPath());
        }
        base.DispatchDraw(canvas);
    }

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        var context = Context;
        if (context is null)
            return;

        if (CrossPlatformMeasure is null)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            return;
        }

        var num = widthMeasureSpec.ToDouble(context);
        var num2 = heightMeasureSpec.ToDouble(context);
        var childAt = GetChildAt(0);
        var size = Size.Zero;
        if (childAt is not null && childAt.Visibility != ViewStates.Gone)
        {
            size = CrossPlatformMeasure(num, num2);
        }

        var defaultSize = GetDefaultSize(
            widthMeasureSpec.GetMode(),
            context.FromPixels(SuggestedMinimumWidth),
            size.Width,
            num
        );

        var defaultSize2 = GetDefaultSize(
            heightMeasureSpec.GetMode(),
            context.FromPixels(SuggestedMinimumHeight),
            size.Height,
            num2
        );

        var num3 = context.ToPixels(defaultSize);
        var num4 = context.ToPixels(defaultSize2);

        SetMeasuredDimension((int)num3, (int)num4);
    }

    protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
    {
        var num = Context.FromPixels(left);
        var num2 = Context.FromPixels(top);
        var num3 = Context.FromPixels(right);
        var num4 = Context.FromPixels(bottom);
        var num5 = num3 - num;
        var num6 = num4 - num2;

        Rect rect = new(0, 0, num5, num6);
        CrossPlatformArrange(rect);
    }

    private static double GetDefaultSize(
        MeasureSpecMode mode,
        double minSize,
        double desiredSize,
        double constraint
    )
    {
        if (mode == MeasureSpecMode.AtMost)
        {
            return Math.Min(Math.Max(minSize, desiredSize), constraint);
        }

        if (mode == MeasureSpecMode.Unspecified)
        {
            return Math.Max(minSize, desiredSize);
        }

        return constraint;
    }
}
