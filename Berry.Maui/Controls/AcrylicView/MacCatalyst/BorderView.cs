using System;
using System.Runtime.InteropServices;
using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace Berry.Maui.Controls;

public class BorderView : UIView
{
    private CAShapeLayer? borderLayer;

    private Thickness borderThickness;

    private Thickness cornerRadius;

    private CGColor borderColor;

    public CGColor BorderColor
    {
        get { return borderColor; }
        set
        {
            borderColor = value;
            SetupBorderLayer();
        }
    }

    public Thickness BorderThickness
    {
        get { return borderThickness; }
        set
        {
            borderThickness = value;
            SetupBorderLayer();
        }
    }

    public Thickness CornerRadius
    {
        get { return cornerRadius; }
        set
        {
            cornerRadius = value;
            SetupBorderLayer();
        }
    }

    internal Func<Rect, Size> CrossPlatformArrange { get; set; }

    internal Func<double, double, Size> CrossPlatformMeasure { get; set; }

    public override CGRect Frame
    {
        get { return base.Frame; }
        set
        {
            base.Frame = value;
            SetupBorderLayer();
        }
    }

    private NFloat CapRadius(double a, double b, double c)
    {
        if (a <= 0)
        {
            return (NFloat)a;
        }
        return (NFloat)Math.Min(a, Math.Min(b, c));
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        var rectangle = Bounds.ToRectangle();
        var crossPlatformMeasure = CrossPlatformMeasure;
        if (crossPlatformMeasure is not null)
        {
            crossPlatformMeasure.Invoke(rectangle.Width, rectangle.Height);
        }
        else { }
        var crossPlatformArrange = CrossPlatformArrange;
        if (crossPlatformArrange is not null)
        {
            crossPlatformArrange.Invoke(rectangle);
        }
        else { }
        SetupBorderLayer();
    }

    public override bool PointInside(CGPoint point, UIEvent uievent)
    {
        var subviews = Subviews;
        for (var i = 0; i < (int)subviews.Length; i++)
        {
            var uIView = subviews[i];
            if (uIView.HitTest(ConvertPointToView(point, uIView), uievent) is not null)
            {
                return true;
            }
        }
        return false;
    }

    private void SetupBorderLayer()
    {
        if (Frame.IsEmpty)
        {
            return;
        }
        if (borderLayer is not null)
        {
            borderLayer.RemoveFromSuperLayer();
            borderLayer = null;
        }
        var layer = Layer;
        layer.BorderWidth = 0;
        layer.CornerRadius = 0;
        var size = layer.Bounds.Size;
        var width = size.Width;
        size = layer.Bounds.Size;
        var height = size.Height;
        var x = layer.Bounds.X;
        var y = layer.Bounds.Y;
        var nFloat = x;
        var nFloat1 = y;
        var nFloat2 = x + width;
        var nFloat3 = y + height;
        var borderThickness = BorderThickness;
        var num = Math.Max(0, borderThickness.Left);
        var num1 = Math.Max(0, borderThickness.Top);
        var num2 = Math.Max(0, borderThickness.Right);
        var num3 = Math.Max(0, borderThickness.Bottom);
        var num4 = num1 + num3;
        var num5 = num + num2;
        var num6 = (num1 > 0 ? num1 * Math.Min(1, height / num4) : num1);
        var num7 = (num2 > 0 ? num2 * Math.Min(1, width / num5) : num2);
        var num8 = (num3 > 0 ? num3 * Math.Min(1, height / num4) : num3);
        var num9 = (num > 0 ? num * Math.Min(1, width / num5) : num);
        var cornerRadius = CornerRadius;
        var left = (NFloat)cornerRadius.Left;
        var top = (NFloat)cornerRadius.Top;
        var right = (NFloat)cornerRadius.Right;
        var bottom = (NFloat)cornerRadius.Bottom;
        var nFloat4 = left + top;
        var nFloat5 = top + right;
        var nFloat6 = right + bottom;
        var nFloat7 = bottom + left;
        var nFloat8 = CapRadius(left, (left / nFloat4) * width, (left / nFloat7) * height);
        var nFloat9 = CapRadius(top, (top / nFloat4) * width, (top / nFloat5) * height);
        var nFloat10 = CapRadius(right, (right / nFloat6) * width, (right / nFloat5) * height);
        var nFloat11 = CapRadius(bottom, (bottom / nFloat6) * width, (bottom / nFloat7) * height);
        var cGPath = new CGPath();
        cGPath.MoveToPoint(nFloat + nFloat8, nFloat1);
        cGPath.AddArcToPoint(nFloat2, nFloat1, nFloat2, nFloat1 + nFloat9, nFloat9);
        cGPath.AddArcToPoint(nFloat2, nFloat3, nFloat2 - nFloat10, nFloat3, nFloat10);
        cGPath.AddArcToPoint(nFloat, nFloat3, nFloat, nFloat3 - nFloat11, nFloat11);
        cGPath.AddArcToPoint(nFloat, nFloat1, nFloat + nFloat8, nFloat1, nFloat8);
        cGPath.CloseSubpath();
        layer.Mask = new CAShapeLayer { Path = cGPath };
        if (num9 > 0 || num6 > 0 || num7 > 0 || num8 > 0)
        {
            CGPath cGPath1 = new();
            cGPath1.AddRect(new CGRect(nFloat, nFloat1, width, height));
            if (num6 > 0 || num9 > 0)
            {
                cGPath1.MoveToPoint(nFloat + nFloat8, (NFloat)(nFloat1 + num6));
            }
            else
            {
                cGPath1.MoveToPoint(nFloat, nFloat1);
            }
            if (num6 > 0 || num7 > 0)
            {
                var num10 = Math.Max(0, nFloat9 - num7);
                var num11 = Math.Max(0, nFloat9 - num6);
                var num12 = Math.Max(num10, num11);
                CGAffineTransform cGAffineTransform =
                    new(
                        (num12 > 0 ? (NFloat)(num10 / num12) : (NFloat)num12),
                        0,
                        0,
                        (num12 > 0 ? (NFloat)(num11 / num12) : (NFloat)num12),
                        (NFloat)(nFloat2 - num7 - num10),
                        (NFloat)(nFloat1 + num6 + num11)
                    );
                cGPath1.AddArc(
                    cGAffineTransform,
                    0,
                    0,
                    (NFloat)num12,
                    (NFloat)4.71238898038469,
                    0,
                    false
                );
            }
            else
            {
                cGPath1.MoveToPoint(nFloat2, nFloat1);
            }
            if (num8 > 0 || num7 > 0)
            {
                var num13 = Math.Max(0, nFloat10 - num7);
                var num14 = Math.Max(0, nFloat10 - num8);
                var num15 = Math.Max(num13, num14);
                CGAffineTransform cGAffineTransform1 =
                    new(
                        (num15 > 0 ? (NFloat)(num13 / num15) : (NFloat)num15),
                        0,
                        0,
                        (num15 > 0 ? (NFloat)(num14 / num15) : (NFloat)num15),
                        (NFloat)(nFloat2 - num7 - num13),
                        (NFloat)(nFloat3 - num8 - num14)
                    );
                cGPath1.AddArc(
                    cGAffineTransform1,
                    0,
                    0,
                    (NFloat)num15,
                    0,
                    (NFloat)1.5707963267949,
                    false
                );
            }
            else
            {
                cGPath1.AddLineToPoint(nFloat2, nFloat3);
            }
            if (num8 > 0 || num9 > 0)
            {
                var nFloat12 = (NFloat)Math.Max(0, nFloat11 - num9);
                var nFloat13 = (NFloat)Math.Max(0, nFloat11 - num8);
                var nFloat14 = (NFloat)Math.Max(nFloat12, nFloat13);
                CGAffineTransform cGAffineTransform2 =
                    new(
                        (nFloat14 > 0 ? nFloat12 / nFloat14 : nFloat14),
                        0,
                        0,
                        (nFloat14 > 0 ? nFloat13 / nFloat14 : nFloat14),
                        (NFloat)(nFloat + num9 + nFloat12),
                        (NFloat)(nFloat3 - num8 - nFloat13)
                    );
                cGPath1.AddArc(
                    cGAffineTransform2,
                    0,
                    0,
                    nFloat14,
                    (NFloat)3.14159265358979 / 2,
                    (NFloat)3.14159265358979,
                    false
                );
            }
            else
            {
                cGPath1.AddLineToPoint(nFloat, nFloat3);
            }
            if (num6 > 0 || num9 > 0)
            {
                var nFloat15 = (NFloat)Math.Max(0, nFloat8 - num9);
                var nFloat16 = (NFloat)Math.Max(0, nFloat8 - num6);
                var nFloat17 = (NFloat)Math.Max(nFloat15, nFloat16);
                var cGAffineTransform3 = new CGAffineTransform(
                    (nFloat17 > 0 ? nFloat15 / nFloat17 : nFloat17),
                    0,
                    0,
                    (nFloat17 > 0 ? nFloat16 / nFloat17 : nFloat17),
                    (NFloat)(nFloat + num9 + nFloat15),
                    (NFloat)(nFloat1 + num6 + nFloat16)
                );
                cGPath1.AddArc(
                    cGAffineTransform3,
                    0,
                    0,
                    nFloat17,
                    (NFloat)3.14159265358979,
                    ((NFloat)3.14159265358979 * 3) / 2,
                    false
                );
            }
            else
            {
                cGPath1.AddLineToPoint(nFloat, nFloat1);
            }
            cGPath1.CloseSubpath();
            var borderColor = BorderColor;
            borderLayer = new CAShapeLayer
            {
                FillRule = CAShapeLayer.FillRuleEvenOdd,
                FillColor = borderColor,
                Path = cGPath1
            };
            layer.AddSublayer(borderLayer);
        }
    }

    public override CGSize SizeThatFits(CGSize size)
    {
        var width = size.Width;
        var height = size.Height;
        return CrossPlatformMeasure.Invoke(width, height).ToCGSize();
    }
}
