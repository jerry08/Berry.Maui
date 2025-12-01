using System;
using Microsoft.Maui.ApplicationModel;

namespace Berry.Maui.Utils;

public static class UniqueDeviceIdentifier
{
    public static string GetUniqueIdentifier() =>
        Android.Provider.Settings.Secure.GetString(
            Platform.AppContext.ContentResolver,
            Android.Provider.Settings.Secure.AndroidId
        )
        ?? throw new ArgumentNullException("Android Id is null");
}
