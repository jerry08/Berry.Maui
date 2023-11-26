using System;
using Microsoft.Maui.Hosting;

namespace Berry.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseBerry(
        this MauiAppBuilder builder,
        Action<HostingBuilder>? configureDelegate = null
    )
    {
        if (configureDelegate is not null)
        {
            configureDelegate(new HostingBuilder(builder));
        }

        return builder;
    }

    public static MauiAppBuilder UseBerry(this MauiAppBuilder builder)
    {
        new HostingBuilder(builder)
            .UseAcrylicView()
            .UseMauiPlainer()
            .UseBottomSheet()
            .UseInsets()
            .UseMaterialSwitch()
            .UseMaterialEntry()
            .UseTabs();

        return builder;
    }
}
