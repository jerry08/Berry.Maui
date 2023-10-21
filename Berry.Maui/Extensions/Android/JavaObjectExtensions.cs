using System;
using System.Diagnostics.CodeAnalysis;
using Android.Runtime;

namespace Berry.Maui.Extensions;

// Copied from https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/Android/JavaObjectExtensions.cs
// to make class public
public static class JavaObjectExtensions
{
    public static bool IsDisposed(this Java.Lang.Object obj)
    {
        return obj.Handle == IntPtr.Zero;
    }

    public static bool IsDisposed(this IJavaObject obj)
    {
        return obj.Handle == IntPtr.Zero;
    }

    public static bool IsAlive([NotNullWhen(true)] this Java.Lang.Object? obj)
    {
        if (obj is null)
            return false;

        return !obj.IsDisposed();
    }

    public static bool IsAlive([NotNullWhen(true)] this Android.Runtime.IJavaObject? obj)
    {
        if (obj is null)
            return false;

        return !obj.IsDisposed();
    }
}
