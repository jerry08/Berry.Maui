using System;
using Google.Android.Material.DatePicker;

namespace Berry.Maui.Utils;

public class MaterialPickerOnPositiveButtonClickListener
    : Java.Lang.Object,
        IMaterialPickerOnPositiveButtonClickListener
{
    public Action<long>? ClickAction;

    public MaterialPickerOnPositiveButtonClickListener() { }

    public MaterialPickerOnPositiveButtonClickListener(Action<long>? action)
    {
        ClickAction = action;
    }

    public void OnPositiveButtonClick(Java.Lang.Object? selectionObj)
    {
        if (selectionObj is null)
        {
            return;
        }

        //if (selectionObj is object ss)
        //{
        //    if (ss is long)
        //    {
        //
        //    }
        //}

        // TODO: Cater for range selection in `MaterialDatePicker.Builder.DateRangePicker`
        ClickAction?.Invoke((long)selectionObj);
    }
}
