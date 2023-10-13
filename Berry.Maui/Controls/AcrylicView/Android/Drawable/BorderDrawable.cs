using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Color = Android.Graphics.Color;
using Paint = Android.Graphics.Paint;
using Path = Android.Graphics.Path;
using RectF = Android.Graphics.RectF;

namespace Berry.Maui.Controls;

public class BorderDrawable : ColorDrawable
{
    private Color borderColor;

    private Color backgroundColor;

    private Thickness borderThickness;

    private Thickness cornerRadius;

    private float borderTopLeftRadius;

    private float borderTopRightRadius;

    private float borderBottomRightRadius;

    private float borderBottomLeftRadius;

    private float borderLeftWidth;

    private float borderTopWidth;

    private float borderRightWidth;

    private float borderBottomWidth;

    public BorderDrawable(Context context, IAcrylicView border)
    {
        // Convert background color to native color
        var nativeColor = GetNativeColor(border.Background.ToColor(), Color.Transparent);

        // Convert border color to native color
        var nativeColor2 = GetNativeColor(border.BorderColor, Color.Transparent);

        Initialize(context, nativeColor2, nativeColor, border.BorderThickness, border.CornerRadius);
    }

    public BorderDrawable(Context context, Thickness cornerRadius, Color color)
    {
        Initialize(context, Colors.Transparent.ToPlatform(), color, new Thickness(0), cornerRadius);
    }

    /// <summary>
    /// Draw background color and rounded corners
    /// </summary>
    /// <param name="canvas"></param>
    public override void Draw(Canvas? canvas)
    {
        if (canvas is null)
            return;

        if (Bounds.IsEmpty)
            return;

        // Draw background color
        if (backgroundColor != 0)
        {
            var paint = new Paint();

            paint.SetStyle(Paint.Style.Fill);

            paint.Color = backgroundColor;

            paint.AntiAlias = true;

            // Draw rounded corners
            var path = new Path();
            path.AddRoundRect(GetBorderInnerRect(), GetBorderInnerRadii(), Path.Direction.Cw);
            canvas.DrawPath(path, paint);
        }

        if (
            borderThickness.Left > 0.0
            || borderThickness.Top > 0.0
            || borderThickness.Right > 0.0
            || borderThickness.Bottom > 0.0
        )
        {
            var paint2 = new Paint() { Color = borderColor };
            paint2.SetStyle(Paint.Style.Fill);
            paint2.AntiAlias = true;
            var path2 = new Path();
            path2.AddRoundRect(GetBorderOuterRect(), GetBorderOuterRadii(), Path.Direction.Cw);
            path2.AddRoundRect(GetBorderInnerRect(), GetBorderInnerRadii(), Path.Direction.Ccw);
            canvas.DrawPath(path2, paint2);
        }
    }

    /// <summary>
    /// Convert Maui colors to native colors
    /// </summary>
    /// <param name="mauiColor"></param>
    /// <param name="nativeDefaultColor"></param>
    /// <returns></returns>
    internal static Color GetNativeColor(
        Microsoft.Maui.Graphics.Color? mauiColor,
        Color nativeDefaultColor
    )
    {
        if (mauiColor is null)
        {
            return nativeDefaultColor;
        }
        return mauiColor.ToPlatform();
    }

    /// <summary>
    /// Get clipping path
    /// </summary>
    /// <returns></returns>
    internal Path GetClipPath()
    {
        var path = new Path();
        path.AddRoundRect(GetBorderInnerRect(), GetBorderInnerRadii(), Path.Direction.Cw);
        return path;
    }

    private void Initialize(
        Context context,
        Color borderColor,
        Color backgroundColor,
        Thickness borderThickness,
        Thickness cornerRadius
    )
    {
        this.borderColor = borderColor;
        this.backgroundColor = backgroundColor;
        this.borderThickness = borderThickness;
        this.cornerRadius = cornerRadius;
        borderTopLeftRadius = context.ToPixels(this.cornerRadius.Left);
        borderTopRightRadius = context.ToPixels(this.cornerRadius.Top);
        borderBottomRightRadius = context.ToPixels(this.cornerRadius.Right);
        borderBottomLeftRadius = context.ToPixels(this.cornerRadius.Bottom);
        borderLeftWidth = context.ToPixels(this.borderThickness.Left);
        borderTopWidth = context.ToPixels(this.borderThickness.Top);
        borderRightWidth = context.ToPixels(this.borderThickness.Right);
        borderBottomWidth = context.ToPixels(this.borderThickness.Bottom);
    }

    /// <summary>
    /// Get the outer rectangle of the border
    /// </summary>
    private RectF GetBorderOuterRect()
    {
        return new RectF(0f, 0f, Bounds.Width(), Bounds.Height());
    }

    /// <summary>
    /// Get the outer radius of the border
    /// </summary>
    private float[] GetBorderOuterRadii()
    {
        return new float[]
        {
            borderTopLeftRadius,
            borderTopLeftRadius,
            borderTopRightRadius,
            borderTopRightRadius,
            borderBottomRightRadius,
            borderBottomRightRadius,
            borderBottomLeftRadius,
            borderBottomLeftRadius
        };
    }

    /// <summary>
    /// Get border lines
    /// </summary>
    private RectF GetBorderInnerRect()
    {
        return new RectF(
            borderLeftWidth,
            borderTopWidth,
            Bounds.Width() - borderRightWidth,
            Bounds.Height() - borderBottomWidth
        );
    }

    /// <summary>
    /// Get the inner radius of the border
    /// </summary>
    private float[] GetBorderInnerRadii()
    {
        return new float[]
        {
            Math.Max(0f, borderTopLeftRadius - borderLeftWidth),
            Math.Max(0f, borderTopLeftRadius - borderTopWidth),
            Math.Max(0f, borderTopRightRadius - borderRightWidth),
            Math.Max(0f, borderTopRightRadius - borderTopWidth),
            Math.Max(0f, borderBottomRightRadius - borderRightWidth),
            Math.Max(0f, borderBottomRightRadius - borderBottomWidth),
            Math.Max(0f, borderBottomLeftRadius - borderLeftWidth),
            Math.Max(0f, borderBottomLeftRadius - borderBottomWidth)
        };
    }
}
