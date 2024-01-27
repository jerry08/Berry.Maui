using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace Berry.Maui.Extensions;

// Copied from https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/ViewExtensions.cs
public static partial class ViewExtensions
{
    public static IMauiContext RequireMauiContext(
        this Element element,
        bool fallbackToAppMauiContext = false
    ) =>
        element.FindMauiContext(fallbackToAppMauiContext)
        ?? throw new InvalidOperationException($"{nameof(IMauiContext)} not found.");

    public static IMauiContext? FindMauiContext(
        this Element element,
        bool fallbackToAppMauiContext = false
    )
    {
        if (element is IElement fe && fe.Handler?.MauiContext != null)
            return fe.Handler.MauiContext;

        foreach (var parent in element.GetParentsPath())
        {
            if (parent is IElement parentView && parentView.Handler?.MauiContext != null)
                return parentView.Handler.MauiContext;
        }

        return fallbackToAppMauiContext ? Application.Current?.FindMauiContext() : default;
    }

    public static IFontManager RequireFontManager(
        this Element element,
        bool fallbackToAppMauiContext = false
    ) =>
        element
            .RequireMauiContext(fallbackToAppMauiContext)
            .Services.GetRequiredService<IFontManager>();

    public static double GetDefaultFontSize(this Element element) =>
        element.FindMauiContext()?.Services?.GetService<IFontManager>()?.DefaultFontSize ?? 0d;

    internal static Element? FindParentWith(
        this Element element,
        Func<Element, bool> withMatch,
        bool includeThis = false
    )
    {
        if (includeThis && withMatch(element))
            return element;

        foreach (var parent in element.GetParentsPath())
        {
            if (withMatch(parent))
                return parent;
        }

        return default;
    }

    public static T? FindParentOfType<T>(this Element element, bool includeThis = false)
        where T : IElement
    {
        if (includeThis && element is T view)
            return view;

        foreach (var parent in element.GetParentsPath())
        {
            if (parent is T parentView)
                return parentView;
        }

        return default;
    }

    public static IList<IGestureRecognizer>? GetCompositeGestureRecognizers(this Element element)
    {
        if (element is IGestureController gc)
            return gc.CompositeGestureRecognizers;

        return null;
    }

    public static IEnumerable<Element> GetParentsPath(this Element self)
    {
        var current = self;

        while (!ApplicationEx.IsApplicationOrNull(current.RealParent))
        {
            current = current.RealParent;
            yield return current;
        }
    }

    public static List<Page> GetParentPages(this Page target)
    {
        var result = new List<Page>();

        var parent = target.RealParent as Page;
        while (!ApplicationEx.IsApplicationOrWindowOrNull(parent))
        {
            result.Add(parent!);
            parent = parent!.RealParent as Page;
        }

        return result;
    }

    public static string? GetStringValue(this IView element)
    {
        string? text = null;

        if (element is ILabel label)
            text = label.Text;
        else if (element is IEntry entry)
            text = entry.Text;
        else if (element is IEditor editor)
            text = editor.Text;
        else if (element is ITimePicker tp)
            text = tp.Time.ToString();
        else if (element is IDatePicker dp)
            text = dp.Date.ToString();
        else if (element is ICheckBox cb)
            text = cb.IsChecked.ToString();
        else if (element is ISwitch sw)
            text = sw.IsOn.ToString();
        else if (element is IRadioButton rb)
            text = rb.IsChecked.ToString();

        return text;
    }

    public static bool TrySetValue(this Element element, string text)
    {
        if (element is Label label)
        {
            label.Text = text;
            return true;
        }
        else if (element is Entry entry)
        {
            entry.Text = text;
            return true;
        }
        else if (element is Editor editor)
        {
            editor.Text = text;
            return true;
        }
        else if (element is CheckBox cb && bool.TryParse(text, out var result))
        {
            cb.IsChecked = result;
            return true;
        }
        else if (element is Switch sw && bool.TryParse(text, out var swResult))
        {
            sw.IsToggled = swResult;
            return true;
        }
        else if (element is RadioButton rb && bool.TryParse(text, out var rbResult))
        {
            rb.IsChecked = rbResult;
            return true;
        }
        else if (element is TimePicker tp && TimeSpan.TryParse(text, out var tpResult))
        {
            tp.Time = tpResult;
            return true;
        }
        else if (element is DatePicker dp && DateTime.TryParse(text, out var dpResult))
        {
            dp.Date = dpResult;
            return true;
        }

        return false;
    }
}
