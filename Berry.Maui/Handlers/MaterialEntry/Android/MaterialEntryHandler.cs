using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Berry.Maui.Controls;
using Berry.Maui.Extensions;
using Google.Android.Material.TextField;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Android.Widget.TextView;
using EditTextExtensions = Berry.Maui.Extensions.EditTextExtensions;

namespace Berry.Maui.Handlers;

public class MaterialEntryHandler : ViewHandler<Entry, RelativeLayout>, IEntryHandler
{
    internal DataFlowDirection DataFlowDirection { get; set; }

    public static IPropertyMapper<IEntry, MaterialEntryHandler> Mapper = new MapperWorkaround<
        IEntry,
        MaterialEntryHandler
    >(EntryHandler.Mapper)
    {
        // Place holder maps to a different property
        [nameof(IEntry.Placeholder)] = MapPlaceHolder,
        [nameof(IEntry.Text)] = MapText,
        [nameof(IEntry.CursorPosition)] = MapCursorPosition,
        [nameof(Entry.BackgroundColor)] = MapBackgroundColor,
        [nameof(IEntry.Width)] = MapWidth,
        [nameof(IEntry.ClearButtonVisibility)] = MapClearButtonVisibility,
        [nameof(MaterialEntry.BoxCornerRadii)] = MapBoxCornerRadii,
    };

    private static void MapBoxCornerRadii(MaterialEntryHandler handler, IEntry arg2)
    {
        if (handler.VirtualView is MaterialEntry materialEntry)
        {
            // https://stackoverflow.com/a/53609949
            handler.TextInputLayout.SetBoxCornerRadii(
                (float)materialEntry.BoxCornerRadii.Left,
                (float)materialEntry.BoxCornerRadii.Top,
                (float)materialEntry.BoxCornerRadii.Right,
                (float)materialEntry.BoxCornerRadii.Bottom
            );
        }
    }

    private static void MapClearButtonVisibility(MaterialEntryHandler handler, IEntry arg2)
    {
        if (handler.TextInputLayout.EditText is null)
            return;

        if (arg2.ClearButtonVisibility is ClearButtonVisibility.WhileEditing)
        {
            handler.TextInputLayout.EndIconVisible = true;
            handler.TextInputLayout.EndIconMode = TextInputLayout.EndIconClearText;

            //var dra = ContextCompat.GetDrawable(handler.TextInputLayout.Context, Resource.Drawable.material_ic_clear_black_24dp);
            ////dra.SetColorFilter(Color.ParseColor("#AE6118"), PorterDuff.Mode.Multiply);
            //handler.TextInputLayout.EndIconDrawable = dra;
        }
        else
        {
            handler.TextInputLayout.EndIconVisible = false;
            handler.TextInputLayout.EndIconMode = TextInputLayout.EndIconNone;
        }

        //handler.TextInputLayout.EditText.SetOnTouchListener(new Listene2());

        //handler.TextInputLayout.SetEndIconOnClickListener(new Listene1(handler.TextInputLayout.EditText));

        // OnFocusChangeListener is cleared after setting EndIconMode programmatically,
        // so set OnFocusChangeListener.
        // https://github.com/material-components/material-components-android/issues/1015
        // https://stackoverflow.com/a/70663379
        handler.TextInputLayout.OnFocusChangeListener = new FocusChangeListenerWorkaround(arg2);
        handler.TextInputLayout.EditText.OnFocusChangeListener = new FocusChangeListenerWorkaround(
            arg2
        );
    }

    private static void MapWidth(MaterialEntryHandler handler, IEntry arg2)
    {
        if (handler.TextInputLayout.EditText is null)
            return;

        //handler.TextInputLayout.EditText.SetWidth(handler.TextInputLayout.Width);
    }

    private static void MapText(MaterialEntryHandler handler, IEntry arg2)
    {
        if (handler.TextInputLayout.EditText is null)
            return;

        if (arg2 is not Entry entry)
            return;

        if (handler.DataFlowDirection == DataFlowDirection.FromPlatform)
        {
            EditTextExtensions.UpdateTextFromPlatform(handler._editText, entry);
            return;
        }

        //handler.TextInputLayout.EditText.Text = arg2.Text;
        handler.TextInputLayout.EditText.UpdateText(arg2);
    }

    private static void MapCursorPosition(MaterialEntryHandler handler, IEntry arg2)
    {
        if (handler.TextInputLayout.EditText is null)
            return;

        handler.TextInputLayout.EditText.UpdateCursorPosition(arg2);
    }

    private static void MapBackgroundColor(MaterialEntryHandler handler, IEntry arg2)
    {
        if (handler.TextInputLayout is null || handler.TextInputLayout.EditText is null)
        {
            return;
        }

        if (arg2 is not Entry entry)
            return;

        if (entry.BackgroundColor is null)
            return;

        //handler.TextInputLayout.EditText.Text = arg2.Text;
        handler.TextInputLayout.SetBackgroundColor(entry.BackgroundColor.ToPlatform());
        handler.TextInputLayout.EditText.SetBackgroundColor(entry.BackgroundColor.ToPlatform());
    }

    private static void MapPlaceHolder(MaterialEntryHandler handler, IEntry arg2)
    {
        if (arg2 is IPlaceholder ph)
            handler.TextInputLayout.Hint = ph.Placeholder;
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        var cursorPosition = _editText.GetCursorPosition();
        var selectedTextLength = _editText.GetSelectedTextLength();

        if (VirtualView.CursorPosition != cursorPosition)
            VirtualView.CursorPosition = cursorPosition;

        if (VirtualView.SelectionLength != selectedTextLength)
            VirtualView.SelectionLength = selectedTextLength;
    }

    public MaterialEntryHandler()
        : base(Mapper, null) { }

    protected override void ConnectHandler(RelativeLayout platformView)
    {
        base.ConnectHandler(platformView);

        _editText.TextChanged += EditText_TextChanged;
        _editText.SelectionChanged += OnSelectionChanged;
        _editText.EditorAction += OnEditorAction;
    }

    private void EditText_TextChanged(object? sender, Android.Text.TextChangedEventArgs e)
    {
        if (TextInputLayout.EditText is null)
            return;

        //VirtualView.UpdateText(TextInputLayout.EditText.Text);

        //if (VirtualView.Text is not null)
        //    VirtualView.CursorPosition = VirtualView.Text.Length;

        if (VirtualView == null)
        {
            return;
        }

        // Let the mapping know that the update is coming from changes to the platform control
        DataFlowDirection = DataFlowDirection.FromPlatform;

        VirtualView.UpdateText(e);

        // Reset to the default direction
        DataFlowDirection = DataFlowDirection.ToPlatform;

        MapClearButtonVisibility(this, VirtualView);
    }

    private void OnEditorAction(object? sender, EditorActionEventArgs e)
    {
        var returnType = VirtualView?.ReturnType;

        // Inside of the android implementations that map events to listeners, the default return value for "Handled" is always true
        // This means, just by subscribing to EditorAction/KeyPressed/etc.. you change the behavior of the control
        // So, we are setting handled to false here in order to maintain default behavior
        bool handled = false;
        if (returnType != null)
        {
            var actionId = e.ActionId;
            var evt = e.Event;
            var currentInputImeFlag = _editText.ImeOptions;

            // On API 34 it looks like they fixed the issue where the actionId is ImeAction.ImeNull when using a keyboard
            // so I'm just setting the actionId here to the current ImeOptions so the logic can all be simplified
            if (
                actionId == Android.Views.InputMethods.ImeAction.ImeNull
                && evt?.KeyCode == Keycode.Enter
            )
            {
                actionId = currentInputImeFlag;
            }

            // keyboard path
            if (evt?.KeyCode == Keycode.Enter && evt?.Action == KeyEventActions.Down)
            {
                handled = true;
            }
            else if (evt?.KeyCode == Keycode.Enter && evt?.Action == KeyEventActions.Up)
            {
                ((IEntry?)VirtualView)?.Completed();
            }
            // InputPaneView Path
            else if (
                evt?.KeyCode is null
                && (
                    actionId == Android.Views.InputMethods.ImeAction.Done
                    || actionId == currentInputImeFlag
                )
            )
            {
                ((IEntry?)VirtualView)?.Completed();
            }
        }

        e.Handled = handled;
    }

    protected override void DisconnectHandler(RelativeLayout platformView)
    {
        base.DisconnectHandler(platformView);

        _editText.TextChanged -= EditText_TextChanged;
        _editText.SelectionChanged -= OnSelectionChanged;
        _editText.EditorAction -= OnEditorAction;
    }

    IEntry IEntryHandler.VirtualView => base.VirtualView;

    AppCompatEditText IEntryHandler.PlatformView => _editText;

    private BerryTextInputEditText _editText = default!;

    public TextInputLayout TextInputLayout { get; set; } = default!;

    protected override RelativeLayout CreatePlatformView()
    {
        //var layoutInflater = MauiContext.Services.GetService<LayoutInflater>();
        var layoutInflater = LayoutInflater.FromContext(Context)!;
        var view = (RelativeLayout)
            layoutInflater.Inflate(Berry.Maui.Resource.Layout.materialentry, null)!;

        TextInputLayout = view.FindViewById<TextInputLayout>(
            Berry.Maui.Resource.Id.materialentry_layout
        )!;

        // https://stackoverflow.com/a/53609949
        //view.SetBoxCornerRadii(30f, 30f, 30f, 30f);

        _editText = view.FindViewById<BerryTextInputEditText>(
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
        TextInputLayout.BoxBackgroundMode = TextInputLayout.BoxBackgroundFilled;
        //TextInputLayout.BoxStrokeWidth = 0;
        //_editText.TextAlignment = Android.Views.TextAlignment.Center;

        return view;
    }
}

public class BerryTextInputEditText : TextInputEditText
{
    public BerryTextInputEditText(Context context)
        : base(context) { }

    public BerryTextInputEditText(Context context, IAttributeSet? attrs)
        : base(context, attrs) { }

    public BerryTextInputEditText(Context context, IAttributeSet? attrs, int defStyleAttr)
        : base(context, attrs, defStyleAttr) { }

    protected BerryTextInputEditText(nint javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer) { }

    public event EventHandler? SelectionChanged;

    protected override void OnSelectionChanged(int selStart, int selEnd)
    {
        base.OnSelectionChanged(selStart, selEnd);

        SelectionChanged?.Invoke(this, EventArgs.Empty);
    }
}

//	Allows mappings to make decisions based on whether the cross-platform properties are updating the platform UI or vice-versa
//  TODO Consider making this public for .NET 8
internal enum DataFlowDirection
{
    ToPlatform,
    FromPlatform
}
