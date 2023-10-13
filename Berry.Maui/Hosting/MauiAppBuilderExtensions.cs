using Berry.Maui.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;

namespace Berry.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseMauiPlainer(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers => handlers.AddPlainer());

        return builder;
    }

    public static MauiAppBuilder UseMaterialSwitch(
        this MauiAppBuilder builder,
        bool applyToAll = true
    )
    {
#if ANDROID && NET7_0_OR_GREATER
        builder.ConfigureMauiHandlers(handlers =>
        {
            if (applyToAll)
            {
                handlers.AddHandler<Switch, Handlers.MaterialSwitchHandler>();
            }
            else
            {
                handlers.AddHandler<MaterialSwitch, Handlers.MaterialSwitchHandler>();
            }
        });
#endif
        return builder;
    }
}
