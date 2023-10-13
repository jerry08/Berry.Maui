using System;

namespace Berry.Maui.Controls;

internal static class JniExtensions
{
    public static bool IsNullOrDisposed(this Java.Lang.Object? javaObject)
    {
        return javaObject is null || javaObject.Handle == IntPtr.Zero;
    }
}
