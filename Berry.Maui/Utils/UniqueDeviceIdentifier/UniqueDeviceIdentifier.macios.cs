using UIKit;

namespace Berry.Maui.Utils;

public static class UniqueDeviceIdentifier
{
    public static string GetUniqueIdentifier() =>
        UIDevice.CurrentDevice.IdentifierForVendor.AsString();
}
