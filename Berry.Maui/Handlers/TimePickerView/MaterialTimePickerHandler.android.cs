using System;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text.Format;
using Berry.Maui.Utils;
using Google.Android.Material.TimePicker;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Berry.Maui.Handlers;

public partial class MaterialTimePickerHandler : TimePickerHandler
{
    MaterialTimePicker? _dialog;

    protected override MauiTimePicker CreatePlatformView()
    {
        var mauiTimePicker = new MauiTimePicker(Context)
        {
            ShowPicker = ShowPickerDialog,
            HidePicker = HidePickerDialog,
        };

        if (Options.UsePlainer)
        {
            using var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
            mauiTimePicker.SetBackground(gradientDrawable);
            mauiTimePicker.BackgroundTintList = ColorStateList.ValueOf(
                Android.Graphics.Color.Transparent
            );
        }

        return mauiTimePicker;
    }

    protected override void DisconnectHandler(MauiTimePicker platformView)
    {
        if (_dialog != null)
        {
            _dialog?.Dismiss();
            _dialog = null;
        }

        base.DisconnectHandler(platformView);
    }

    void ShowPickerDialog()
    {
        if (VirtualView is null)
            return;

        var time = VirtualView.Time;
        ShowPickerDialog(time.Hours, time.Minutes);
    }

    void ShowPickerDialog(int hour, int minute)
    {
        if (VirtualView is null || PlatformView is null)
            return;

        var fragmentManager = Platform.CurrentActivity?.GetFragmentManager();
        if (fragmentManager is null)
            return;

        var builder = new MaterialTimePicker.Builder()
            .SetTimeFormat(Use24HourView ? TimeFormat.Clock24h : TimeFormat.Clock12h)
            .SetInputMode(MaterialTimePicker.InputModeClock)
            .SetMinute(minute)
            .SetHour(hour);

        _dialog = builder.Build();
        _dialog.AddOnPositiveButtonClickListener(
            new ViewClickListener(() =>
            {
                VirtualView.Time = new TimeSpan(_dialog.Hour, _dialog.Minute, 0);
                VirtualView.IsFocused = false;
            })
        );
        _dialog.Show(fragmentManager, "TIME_PICKER");
    }

    void HidePickerDialog()
    {
        _dialog?.Dismiss();
        _dialog = null;
    }

    bool Use24HourView =>
        VirtualView != null
        && (
            DateFormat.Is24HourFormat(PlatformView?.Context) && VirtualView.Format == "t"
            || VirtualView.Format == "HH:mm"
        );
}
