using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Berry.Maui.Controls;

public partial class OutlinedMaterialEntry
{
    private ImageSource? tempIcon;

    // BindableProperties
    public ClearButtonVisibility ClearButtonVisibility
    {
        get => (ClearButtonVisibility)GetValue(ClearButtonVisibilityProperty);
        set => SetValue(ClearButtonVisibilityProperty, value);
    }

    public static readonly BindableProperty ClearButtonVisibilityProperty = BindableProperty.Create(
        nameof(ClearButtonVisibility),
        typeof(ClearButtonVisibility),
        typeof(OutlinedMaterialEntry),
        ClearButtonVisibility.Never,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            var clearButtonVisibility = (ClearButtonVisibility)newValue;
            if (clearButtonVisibility == ClearButtonVisibility.WhileEditing)
            {
                if (view.customEntry.Text?.Length > 0)
                    view.clearButtonImage.IsVisible = true;
            }
            else
            {
                view.clearButtonImage.IsVisible = false;
            }
        }
    );

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(OutlinedMaterialEntry),
        Colors.White,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.customEntry.TextColor = (Color)newValue;
        }
    );

    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
        nameof(PlaceholderColor),
        typeof(Color),
        typeof(OutlinedMaterialEntry),
        Colors.White,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.placeholderText.TextColor = (Color)newValue;
        }
    );

    public Color CounterTextColor
    {
        get => (Color)GetValue(CounterTextColorProperty);
        set => SetValue(CounterTextColorProperty, value);
    }

    public static readonly BindableProperty CounterTextColorProperty = BindableProperty.Create(
        nameof(CounterTextColor),
        typeof(Color),
        typeof(OutlinedMaterialEntry),
        Colors.Gray,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.charCounterText.TextColor = (Color)newValue;
        }
    );

    public Color HelperTextColor
    {
        get => (Color)GetValue(HelperTextColorProperty);
        set => SetValue(HelperTextColorProperty, value);
    }

    public static readonly BindableProperty HelperTextColorProperty = BindableProperty.Create(
        nameof(HelperTextColor),
        typeof(Color),
        typeof(OutlinedMaterialEntry),
        Colors.Gray,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.helperText.TextColor = (Color)newValue;
        }
    );

    public Color PlaceholderBackgroundColor
    {
        get => (Color)GetValue(PlaceholderBackgroundColorProperty);
        set => SetValue(PlaceholderBackgroundColorProperty, value);
    }

    public static readonly BindableProperty PlaceholderBackgroundColorProperty =
        BindableProperty.Create(
            nameof(PlaceholderBackgroundColor),
            typeof(Color),
            typeof(OutlinedMaterialEntry),
            Colors.White,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedMaterialEntry)bindable;
                view.placeholderContainer.BackgroundColor = (Color)newValue;
            }
        );

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(OutlinedMaterialEntry),
        default(string),
        BindingMode.TwoWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.customEntry.Text = (string)newValue;
        }
    );

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder),
        typeof(string),
        typeof(OutlinedMaterialEntry),
        default(string),
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.placeholderText.Text = (string)newValue;
        }
    );

    public static readonly BindableProperty HelperTextProperty = BindableProperty.Create(
        nameof(HelperText),
        typeof(string),
        typeof(OutlinedMaterialEntry),
        default(string),
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.helperText.Text = (string)newValue;
            if (view.errorText.IsVisible)
                view.helperText.IsVisible = false;
            else
                view.helperText.IsVisible = !string.IsNullOrEmpty(view.helperText.Text);
        }
    );

    public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
        nameof(ErrorText),
        typeof(string),
        typeof(OutlinedMaterialEntry),
        default(string),
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.errorText.Text = (string)newValue;
        }
    );

    public static readonly BindableProperty LeadingIconProperty = BindableProperty.Create(
        nameof(LeadingIcon),
        typeof(ImageSource),
        typeof(OutlinedMaterialEntry),
        null,
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.leadingIcon.Source = (ImageSource)newValue;
            view.leadingIcon.IsVisible = !view.leadingIcon.Source.IsEmpty;
        }
    );

    public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(
        nameof(TrailingIcon),
        typeof(ImageSource),
        typeof(OutlinedMaterialEntry),
        null,
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.trailingIcon.Source = (ImageSource?)newValue;
            view.trailingIcon.IsVisible = view.trailingIcon.Source != null;
        }
    );

    public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
        nameof(HasError),
        typeof(bool),
        typeof(OutlinedMaterialEntry),
        default(bool),
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.errorText.IsVisible = (bool)newValue;
            view.containerBorder.Stroke = view.errorText.IsVisible
                ? Colors.Red
                : (view.customEntry.IsFocused ? view.FocusedStroke : view.UnfocusedStroke);
            view.helperText.IsVisible = !view.errorText.IsVisible;
            view.placeholderText.TextColor = view.errorText.IsVisible
                ? Colors.Red
                : view.PlaceholderColor;
            view.Placeholder = view.errorText.IsVisible ? $"{view.Placeholder}*" : view.Placeholder;
            if (view.TrailingIcon != null && !view.TrailingIcon.IsEmpty)
                view.tempIcon = view.TrailingIcon;
            view.TrailingIcon = view.errorText.IsVisible
                ? ImageSource.FromFile("ic_error.png")
                : view.tempIcon;
            view.trailingIcon.IsVisible = view.errorText.IsVisible;
        }
    );

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword),
        typeof(bool),
        typeof(OutlinedMaterialEntry),
        default(bool),
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.customEntry.IsPassword = (bool)newValue;
            view.passwordIcon.IsVisible = (bool)newValue;
        }
    );

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        nameof(MaxLength),
        typeof(int),
        typeof(OutlinedMaterialEntry),
        default(int),
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.customEntry.MaxLength = (int)newValue;
            view.charCounterText.IsVisible = view.customEntry.MaxLength > 0;
            view.charCounterText.Text = $"0 / {view.MaxLength}";
        }
    );

    public static readonly BindableProperty FocusedStrokeProperty = BindableProperty.Create(
        nameof(FocusedStroke),
        typeof(Color),
        typeof(OutlinedMaterialEntry),
        Colors.Blue,
        BindingMode.OneWay
    );

    public static readonly BindableProperty UnfocusedStrokeProperty = BindableProperty.Create(
        nameof(UnfocusedStroke),
        typeof(Color),
        typeof(OutlinedMaterialEntry),
        Colors.Gray,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.containerBorder.Stroke = (Color)newValue;
        }
    );

    public static readonly BindableProperty BorderStrokeShapeProperty = BindableProperty.Create(
        nameof(BorderStrokeShape),
        typeof(IShape),
        typeof(OutlinedMaterialEntry),
        null,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.containerBorder.StrokeShape = (IShape?)newValue;
        }
    );

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
        nameof(ReturnCommand),
        typeof(ICommand),
        typeof(OutlinedMaterialEntry),
        default(ICommand),
        BindingMode.OneWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var view = (OutlinedMaterialEntry)bindable;
            view.customEntry.ReturnCommand = (ICommand)newValue;
        }
    );

    public ICommand ClearCommand { get; }

    public OutlinedMaterialEntry()
    {
        ClearCommand = new Command(() =>
        {
            customEntry.Text = string.Empty;
        });

        InitializeComponent();

        customEntry.Text = Text;

        customEntry.TextChanged += OnCustomEntryTextChanged;

        customEntry.Completed += OnCustomEntryCompleted;
    }

    // Event Handlers
    public event EventHandler<EventArgs>? EntryCompleted;

    public event EventHandler<TextChangedEventArgs>? TextChanged;

    // Properties
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string HelperText
    {
        get => (string)GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }

    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }

    public ImageSource? LeadingIcon
    {
        get => (ImageSource?)GetValue(LeadingIconProperty);
        set => SetValue(LeadingIconProperty, value);
    }

    public ImageSource? TrailingIcon
    {
        get => (ImageSource?)GetValue(TrailingIconProperty);
        set => SetValue(TrailingIconProperty, value);
    }

    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public Color FocusedStroke
    {
        get => (Color)GetValue(FocusedStrokeProperty);
        set => SetValue(FocusedStrokeProperty, value);
    }

    public Color UnfocusedStroke
    {
        get => (Color)GetValue(UnfocusedStrokeProperty);
        set => SetValue(UnfocusedStrokeProperty, value);
    }

    public IShape? BorderStrokeShape
    {
        get => (IShape?)GetValue(BorderStrokeShapeProperty);
        set => SetValue(BorderStrokeShapeProperty, value);
    }

    public Keyboard Keyboard
    {
        set => customEntry.Keyboard = value;
    }

    public ReturnType ReturnType
    {
        set => customEntry.ReturnType = value;
    }

    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    // Here we check if there is any text on the entry,
    // if not, we set the border and placeholder text color
    // and activate the animation to move the placeholder up
    private async Task ControlFocused()
    {
        if (string.IsNullOrEmpty(customEntry.Text) || customEntry.Text.Length > 0)
        {
            customEntry.Focus();

            containerBorder.Stroke = HasError ? Colors.Red : FocusedStroke;
            placeholderText.TextColor = HasError ? Colors.Red : PlaceholderColor;

            //var y = DeviceInfo.Platform == DevicePlatform.WinUI ? -25 : -22;
            var y = -(customEntry.Height / 2);

            await placeholderContainer.TranslateTo(0, y, 120, Easing.CubicOut);

            placeholderContainer.HorizontalOptions = LayoutOptions.Start;
            //this.placeholderText.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            placeholderText.FontSize = 12;
        }
        else
            await ControlUnfocused();
    }

    // Here we change the border and placeholder text color
    // back to normal and check if there is any text on the entry,
    // if not we launch the animation to place the placeholder
    // back over the entry
    private async Task ControlUnfocused()
    {
        containerBorder.Stroke = HasError ? Colors.Red : UnfocusedStroke;
        placeholderText.TextColor = HasError ? Colors.Red : PlaceholderColor;

        customEntry.Unfocus();

        if (string.IsNullOrEmpty(customEntry.Text) || customEntry.MaxLength <= 0)
        {
            await placeholderContainer.TranslateTo(0, 0, 100, Easing.CubicOut);

            placeholderContainer.HorizontalOptions = LayoutOptions.Start;
            //this.placeholderText.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            placeholderText.FontSize = 12;
        }
    }

    private void CustomEntryFocused(object? sender, FocusEventArgs e)
    {
        if (e.IsFocused)
            MainThread.BeginInvokeOnMainThread(async () => await ControlFocused());
    }

    private void CustomEntryUnfocused(object? sender, FocusEventArgs e)
    {
        if (!e.IsFocused)
            MainThread.BeginInvokeOnMainThread(async () => await ControlUnfocused());
    }

    private void OutlinedMaterialEntryTapped(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () => await ControlFocused());
    }

    // Here we change the password type of the entry
    // in order to change the eye icon
    private void PasswordEyeTapped(object? sender, EventArgs e)
    {
        customEntry.IsPassword = !customEntry.IsPassword;
    }

    // Here we set the text by every new char
    // and update the charCounter label
    private void OnCustomEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        Text = e.NewTextValue;

        if (charCounterText.IsVisible)
            charCounterText.Text = $"{customEntry.Text.Length} / {MaxLength}";

        if (
            ClearButtonVisibility == ClearButtonVisibility.WhileEditing
            && customEntry.Text?.Length > 0
        )
        {
            clearButtonImage.IsVisible = true;
        }
        else
        {
            clearButtonImage.IsVisible = false;
        }

        TextChanged?.Invoke(this, e);
    }

    private void OnCustomEntryCompleted(object? sender, EventArgs e)
    {
        EntryCompleted?.Invoke(this, EventArgs.Empty);
    }
}
