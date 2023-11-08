using System;
using Android.Content;
using Android.Graphics;
using Android.Renderscripts;

namespace Berry.Maui.Extensions;

public static class BitmapExtensions
{
    private static readonly float BITMAP_SCALE = 0.4f;

    public static Bitmap Blur(this Context context, Bitmap image, float radius = 25f)
    {
        var width = (int)Math.Round(image.Width * BITMAP_SCALE);
        var height = (int)Math.Round(image.Height * BITMAP_SCALE);

        if (width <= 0 || height <= 0)
            return image;

        //int width = image.Width;
        //int height = image.Height;

        var inputBitmap = Bitmap.CreateScaledBitmap(image, width, height, false);
        if (inputBitmap is null)
            return image;

        var outputBitmap = Bitmap.CreateBitmap(inputBitmap);
        if (outputBitmap is null)
            return image;

        var rendersScript = RenderScript.Create(context);

        var intrinsicBlur = ScriptIntrinsicBlur.Create(rendersScript, Element.U8_4(rendersScript));
        if (intrinsicBlur is null)
            return image;

        var tmpIn = Allocation.CreateFromBitmap(rendersScript, inputBitmap);
        var tmpOut = Allocation.CreateFromBitmap(rendersScript, outputBitmap);
        if (tmpOut is null)
            return image;

        intrinsicBlur.SetRadius(radius);
        intrinsicBlur.SetInput(tmpIn);
        intrinsicBlur.ForEach(tmpOut);
        tmpOut.CopyTo(outputBitmap);

        return outputBitmap;
    }
}
