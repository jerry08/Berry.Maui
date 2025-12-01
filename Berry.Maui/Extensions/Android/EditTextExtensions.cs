using Android.Text;
using Android.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Berry.Maui.Extensions;

public static class EditTextExtensions
{
    static (string oldText, string newText) GetTexts(EditText editText, InputView inputView)
    {
        var oldText = editText.Text ?? string.Empty;

        var inputType = editText.InputType;

        var isPasswordEnabled =
            (inputType & InputTypes.TextVariationPassword) == InputTypes.TextVariationPassword
            || (inputType & InputTypes.NumberVariationPassword)
                == InputTypes.NumberVariationPassword;

        var newText = TextTransformUtilities.GetTransformedText(
            inputView?.Text,
            isPasswordEnabled ? TextTransform.None : inputView.TextTransform
        );

        return (oldText, newText);
    }

    internal static void UpdateTextFromPlatform(this EditText editText, InputView inputView)
    {
        (var oldText, var newText) = GetTexts(editText, inputView);

        if (oldText != newText)
        {
            // This update is happening while inputting text into the EditText, so we want to avoid
            // resettting the cursor position and selection
            editText.SetTextKeepState(newText);
        }
    }
}
