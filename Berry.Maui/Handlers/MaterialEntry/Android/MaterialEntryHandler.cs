using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.TextField;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

namespace Berry.Maui.Handlers;

public class MaterialEntryHandler : ViewHandler<Entry, TextInputLayout>, IEntryHandler
{
    public static IPropertyMapper<IEntry, MaterialEntryHandler> Mapper = new MapperWorkaround<
        IEntry,
        MaterialEntryHandler
    >(EntryHandler.Mapper)
    {
        // Place holder maps to a different property
        [nameof(IEntry.Placeholder)] = MapPlaceHolder,
        [nameof(IEntry.Text)] = MapText,
        [nameof(IEntry.Width)] = MapWidth,
        [nameof(IEntry.ClearButtonVisibility)] = MapClearButtonVisibility,
    };

    private static void MapClearButtonVisibility(MaterialEntryHandler arg1, IEntry arg2)
    {
        if (arg1.PlatformView.EditText is null)
            return;

        arg1.PlatformView.EndIconVisible =
            arg2.ClearButtonVisibility == ClearButtonVisibility.WhileEditing;
        arg1.PlatformView.EndIconMode = TextInputLayout.EndIconClearText;

        //arg1.PlatformView.EditText.SetOnTouchListener(new Listene2());

        //arg1.PlatformView.SetEndIconOnClickListener(new Listene1(arg1.PlatformView.EditText));

        // OnFocusChangeListener is cleared after setting EndIconMode programmatically,
        // so set OnFocusChangeListener.
        // https://github.com/material-components/material-components-android/issues/1015
        // https://stackoverflow.com/a/70663379
        arg1.PlatformView.OnFocusChangeListener = new FocusChangeListenerWorkaround(arg2);
        arg1.PlatformView.EditText.OnFocusChangeListener = new FocusChangeListenerWorkaround(arg2);
    }

    private static void MapWidth(MaterialEntryHandler arg1, IEntry arg2)
    {
        if (arg1.PlatformView.EditText is null)
            return;

        arg1.PlatformView.EditText.SetWidth(arg1.PlatformView.Width);
    }

    private static void MapText(MaterialEntryHandler arg1, IEntry arg2)
    {
        if (arg1.PlatformView.EditText is null)
            return;

        arg1.PlatformView.EditText.Text = arg2.Text;
    }

    private static void MapPlaceHolder(MaterialEntryHandler arg1, IEntry arg2)
    {
        if (arg2 is IPlaceholder ph)
            arg1.PlatformView.Hint = ph.Placeholder;
    }

    public MaterialEntryHandler()
        : base(Mapper, null) { }

    protected override void ConnectHandler(TextInputLayout platformView)
    {
        base.ConnectHandler(platformView);

        platformView.LayoutChange += PlatformView_LayoutChange;
    }

    protected override void DisconnectHandler(TextInputLayout platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.LayoutChange -= PlatformView_LayoutChange;
    }

    private void PlatformView_LayoutChange(
        object? sender,
        Android.Views.View.LayoutChangeEventArgs e
    )
    {
        if (PlatformView.EditText is null)
            return;

        PlatformView.EditText.SetWidth(PlatformView.Width);
    }

    IEntry IEntryHandler.VirtualView => base.VirtualView as IEntry;

    AppCompatEditText IEntryHandler.PlatformView => _editText;

    AppCompatEditText _editText;

    protected override TextInputLayout CreatePlatformView()
    {
        //var layoutInflater = MauiContext.Services.GetService<LayoutInflater>();
        var layoutInflater = LayoutInflater.FromContext(Context)!;
        var view = (TextInputLayout)
            layoutInflater.Inflate(Berry.Maui.Resource.Layout.materialentry, null)!;

        // https://stackoverflow.com/a/53609949
        view.SetBoxCornerRadii(30f, 30f, 30f, 30f);

        _editText = view.FindViewById<TextInputEditText>(
            Berry.Maui.Resource.Id.materialentry_entry
        )!;

        return view;
    }
}
