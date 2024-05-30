using System;
using Android.Graphics.Drawables;
using Berry.Maui.Utils;
using Google.Android.Material.DatePicker;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Android.App.DatePickerDialog;

namespace Berry.Maui.Handlers.DatePickerView;

public partial class MaterialDatePickerHandler : DatePickerHandler
{
    MaterialStyledDatePickerDialog? _dialog;

    protected override MauiDatePicker CreatePlatformView()
    {
        var mauiDatePicker = new MauiDatePicker(Context)
        {
            ShowPicker = ShowPickerDialog,
            HidePicker = HidePickerDialog
        };

        using (var gradientDrawable = new GradientDrawable())
        {
            gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
            mauiDatePicker.SetBackground(gradientDrawable);
            mauiDatePicker.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(
                Android.Graphics.Color.Transparent
            );
        }

        var date = VirtualView?.Date;

        if (date != null)
            _dialog = CreateDatePickerDialog2(date.Value.Year, date.Value.Month, date.Value.Day);

        return mauiDatePicker;
    }

    void ShowPickerDialog()
    {
        if (VirtualView == null)
            return;

        if (_dialog != null && _dialog.IsShowing)
            return;

        var date = VirtualView.Date;
        ShowPickerDialog(date.Year, date.Month - 1, date.Day);
    }

    void ShowPickerDialog(int year, int month, int day)
    {
        var date = VirtualView?.Date;

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

        if (date is not null)
        {
            var dateTimeOffset = new DateTimeOffset(date.Value);
            builder.SetSelection(dateTimeOffset.ToUnixTimeMilliseconds());
        }

        var mPicker = builder.Build();
        mPicker.AddOnPositiveButtonClickListener(
            new MaterialPickerOnPositiveButtonClickListener(
                (selection) =>
                {
                    if (VirtualView is not null)
                        VirtualView.Date = DateTimeOffset.FromUnixTimeMilliseconds(selection).Date;
                }
            )
        );
        mPicker.Show(fragmentManager, "DATE_PICKER");
    }

    void HidePickerDialog()
    {
        _dialog?.Hide();
    }

    protected virtual MaterialStyledDatePickerDialog CreateDatePickerDialog2(
        int year,
        int month,
        int day
    )
    {
        var dialog = new MaterialStyledDatePickerDialog(
            Context!,
            new MaterialDateSetListener(this),
            year,
            month,
            day
        );

        return dialog;
    }
}

public class MaterialDateSetListener : Java.Lang.Object, IOnDateSetListener
{
    MaterialDatePickerHandler _handler;

    public MaterialDateSetListener(MaterialDatePickerHandler handler)
    {
        _handler = handler;
    }

    public void OnDateSet(Android.Widget.DatePicker? view, int year, int month, int dayOfMonth)
    {
        if (_handler.VirtualView != null)
        {
            _handler.VirtualView.Date = new DateTime(year, month, dayOfMonth);
        }
    }
}
