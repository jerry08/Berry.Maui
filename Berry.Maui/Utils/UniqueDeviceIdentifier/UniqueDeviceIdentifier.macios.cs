using UIKit;

namespace Berry.Maui.Utils;

public static class UniqueDeviceIdentifier
{
    public static string GetUniqueIdentifier()
    {
        return UIDevice.CurrentDevice.IdentifierForVendor.AsString().Replace("-", "") + "-iOS";
    }
}
