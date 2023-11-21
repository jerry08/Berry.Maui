using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Berry.Maui.Controls;
using Google.Android.Material.TextField;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

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
        [nameof(MaterialEntry.BoxCornerRadii)] = MapBoxCornerRadii,
    };

    private static void MapBoxCornerRadii(MaterialEntryHandler arg1, IEntry arg2)
    {
        if (arg1.VirtualView is MaterialEntry materialEntry)
        {
            // https://stackoverflow.com/a/53609949
            arg1.PlatformView.SetBoxCornerRadii(
                (float)materialEntry.BoxCornerRadii.Left,
                (float)materialEntry.BoxCornerRadii.Top,
                (float)materialEntry.BoxCornerRadii.Right,
                (float)materialEntry.BoxCornerRadii.Bottom
            );
        }
    }

    private static void MapClearButtonVisibility(MaterialEntryHandler arg1, IEntry arg2)
    {
        if (arg1.PlatformView.EditText is null)
            return;

        if (arg2.ClearButtonVisibility is ClearButtonVisibility.WhileEditing)
        {
            arg1.PlatformView.EndIconVisible = true;
            arg1.PlatformView.EndIconMode = TextInputLayout.EndIconClearText;

            //var dra = ContextCompat.GetDrawable(arg1.PlatformView.Context, Resource.Drawable.material_ic_clear_black_24dp);
            ////dra.SetColorFilter(Color.ParseColor("#AE6118"), PorterDuff.Mode.Multiply);
            //arg1.PlatformView.EndIconDrawable = dra;
        }
        else
        {
            arg1.PlatformView.EndIconVisible = false;
            arg1.PlatformView.EndIconMode = TextInputLayout.EndIconNone;
        }

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

        //platformView.LayoutChange += PlatformView_LayoutChange;

        Application.Current?.Dispatcher.Dispatch(() =>
        {
            PlatformView.EditText?.SetWidth(PlatformView.Width);
        });
    }

    protected override void DisconnectHandler(TextInputLayout platformView)
    {
        base.DisconnectHandler(platformView);

        //platformView.LayoutChange -= PlatformView_LayoutChange;
    }

    private void PlatformView_LayoutChange(
        object? sender,
        Android.Views.View.LayoutChangeEventArgs e
    )
    {
        if (PlatformView.EditText is null)
            return;

        PlatformView.EditText.SetWidth(PlatformView.Width);

        //var test = PlatformView.GetChildrenOfType<AppCompatTextView>().ToList();
        //
        //for (var i = 0; i < test.Count; i++)
        //{
        //    test[i].SetWidth(PlatformView.Width);
        //}
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
        //view.SetBoxCornerRadii(30f, 30f, 30f, 30f);

        _editText = view.FindViewById<TextInputEditText>(
            Berry.Maui.Resource.Id.materialentry_entry
        )!;

        //view.LayoutParameters = new FrameLayout.LayoutParams(
        //    ViewGroup.LayoutParams.MatchParent,
        //    ViewGroup.LayoutParams.MatchParent
        //);
        //
        //_editText.LayoutParameters = new FrameLayout.LayoutParams(
        //    ViewGroup.LayoutParams.MatchParent,
        //    ViewGroup.LayoutParams.MatchParent
        //);

        foreach (var childView in view.GetChildrenOfType<AppCompatTextView>())
        {
            childView.LayoutParameters = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
            );
        }

        // This is set to default in Theme.Material3.DayNight but not other themes.
        // Theme.Material3.DayNight underline is removed also.
        view.BoxBackgroundMode = TextInputLayout.BoxBackgroundOutline;
        //view.BoxStrokeWidth = 0;
        //_editText.TextAlignment = Android.Views.TextAlignment.Center;

        return view;
    }
}
