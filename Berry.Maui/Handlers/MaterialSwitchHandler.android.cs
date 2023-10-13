#if NET7_0_OR_GREATER
using Microsoft.Maui.Handlers;
using ASwitch = Google.Android.Material.MaterialSwitch.MaterialSwitch;

namespace Berry.Maui.Handlers;

public partial class MaterialSwitchHandler : SwitchHandler
{
    protected override ASwitch CreatePlatformView()
    {
        return new ASwitch(Context);
    }
}
#endif
