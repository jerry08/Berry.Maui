using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Path = Android.Graphics.Path;
using RectF = Android.Graphics.RectF;

namespace Berry.Maui.Controls;

public class CornerFrameLayout : FrameLayout
{
    public CornerFrameLayout(Context context, IAttributeSet attrs)
        : base(context, attrs)
    {
        SetClipChildren(true);
    }

    public CornerFrameLayout(Context context)
        : base(context)
    {
        SetClipChildren(true);
    }

    private readonly Path _mPath = new Path();
    private readonly float[] _mRadii = new float[8];

    protected override void OnDraw(Canvas? canvas)
    {
        base.OnDraw(canvas);

        if (!_mPath.IsEmpty)
            canvas?.ClipPath(_mPath);
    }

    protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
    {
        base.OnSizeChanged(w, h, oldw, oldh);
        _mPath.Reset();
        _mPath.AddRoundRect(new RectF(0, 0, w, h), _mRadii, Path.Direction.Cw);
    }

    /// <summary>
    /// Set each corner radius.
    /// </summary>
    /// <param name="topLeft"></param>
    /// <param name="topRight"></param>
    /// <param name="bottomRight"></param>
    /// <param name="bottomLeft"></param>
    public void SetRadius(float topLeft, float topRight, float bottomRight, float bottomLeft)
    {
        _mRadii[0] = topLeft;
        _mRadii[1] = topLeft;

        _mRadii[2] = topRight;
        _mRadii[3] = topRight;

        _mRadii[4] = bottomRight;
        _mRadii[5] = bottomRight;

        _mRadii[6] = bottomLeft;
        _mRadii[7] = bottomLeft;

        Invalidate();
    }

    /// <summary>
    /// Set each corner radius.
    /// </summary>
    public void SetRadius(
        float topLeftX,
        float topLeftY,
        float topRightX,
        float topRightY,
        float bottomRightX,
        float bottomRightY,
        float bottomLeftX,
        float bottomLeftY
    )
    {
        _mRadii[0] = topLeftX;
        _mRadii[1] = topLeftY;

        _mRadii[2] = topRightX;
        _mRadii[3] = topRightY;

        _mRadii[4] = bottomRightX;
        _mRadii[5] = bottomRightY;

        _mRadii[6] = bottomLeftX;
        _mRadii[7] = bottomLeftY;

        Invalidate();
    }
}
