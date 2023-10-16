using Microsoft.Maui.ApplicationModel;

namespace Berry.Maui.Utils.UniqueDeviceIdentifier;

public static class UniqueDeviceIdentifier
{
    public static string GetUniqueIdentifier()
    {
        return Android.Provider.Settings.Secure.GetString(
                Platform.AppContext.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId
            ) + "-Android"
            ?? "";
    }
}
