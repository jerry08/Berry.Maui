using Microsoft.Maui;

namespace Berry.Maui.Handlers;

public class FocusChangeListenerWorkaround
    : Java.Lang.Object,
        Android.Views.View.IOnFocusChangeListener
{
    private readonly IEntry _entry;

    public FocusChangeListenerWorkaround(IEntry entry)
    {
        _entry = entry;
    }

    public void OnFocusChange(Android.Views.View? v, bool hasFocus)
    {
        _entry.IsFocused = hasFocus;
    }
}
