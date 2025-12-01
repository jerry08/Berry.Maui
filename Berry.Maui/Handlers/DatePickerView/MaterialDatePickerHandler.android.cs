using System;
using Android.Graphics.Drawables;
using Berry.Maui.Utils;
using Google.Android.Material.DatePicker;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Berry.Maui.Handlers;

public partial class MaterialDatePickerHandler : DatePickerHandler
{
    MaterialDatePicker? _dialog;

    protected override MauiDatePicker CreatePlatformView()
    {
        var mauiDatePicker = new MauiDatePicker(Context)
        {
            ShowPicker = ShowPickerDialog,
            HidePicker = HidePickerDialog,
        };

        if (Options.UsePlainer)
        {
            using var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
            mauiDatePicker.SetBackground(gradientDrawable);
            mauiDatePicker.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(
                Android.Graphics.Color.Transparent
            );
        }

        return mauiDatePicker;
    }

    protected override void DisconnectHandler(MauiDatePicker platformView)
    {
        if (_dialog != null)
        {
            _dialog.Dismiss();
            _dialog.Dispose();
            _dialog = null;
        }

        base.DisconnectHandler(platformView);
    }

    void ShowPickerDialog()
    {
        if (VirtualView is null)
            return;

        if (_dialog != null && _dialog.IsVisible)
            return;

        ShowPickerDialog(VirtualView.Date);
    }

    void ShowPickerDialog(DateTime? date)
    {
        if (VirtualView?.Date is null)
            return;

        var fragmentManager = Platform.CurrentActivity?.GetFragmentManager();
        if (fragmentManager is null)
            return;

        //var constrainsBuilder = new CalendarConstraints.Builder()
        //    .SetStart(1)
        //    .SetEnd(1)
        //    .SetOpenAt(1);

        var builder = MaterialDatePicker.Builder.DatePicker();
        //.SetCalendarConstraints(constrainsBuilder.Build());
        //.SetTitleText("Select date of birth");

        builder.SetSelection(new DateTimeOffset(date ?? DateTime.Today).ToUnixTimeMilliseconds());

        _dialog = builder.Build();
        _dialog.AddOnPositiveButtonClickListener(
            new MaterialPickerOnPositiveButtonClickListener(
                (selection) =>
                {
                    VirtualView?.Date = DateTimeOffset.FromUnixTimeMilliseconds(selection).Date;
                }
            )
        );
        _dialog.Show(fragmentManager, "DATE_PICKER");
    }

    void HidePickerDialog()
    {
        _dialog?.Dismiss();
    }
}
