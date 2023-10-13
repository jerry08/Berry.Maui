using Berry.Maui.Handlers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;

namespace Berry.Maui;

public static class MauiHandlersCollectionExtensions
{
    public static IMauiHandlersCollection AddPlainer(this IMauiHandlersCollection handlers)
    {
        handlers
            .AddHandler(typeof(Entry), typeof(EntryViewHandler))
            .AddHandler(typeof(Editor), typeof(EditorViewHandler))
            .AddHandler(typeof(Picker), typeof(PickerViewHandler))
            .AddHandler(typeof(DatePicker), typeof(DatePickerViewHandler))
            .AddHandler(typeof(TimePicker), typeof(TimePickerViewHandler));

        return handlers;
    }
}
