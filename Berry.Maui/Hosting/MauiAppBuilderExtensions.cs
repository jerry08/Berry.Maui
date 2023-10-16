using Berry.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace Berry.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseBerry(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddPlainer();
            handlers.AddHandler(typeof(AcrylicView), typeof(AcrylicViewHandler));
        });

        builder.UseMaterialSwitch();

        return builder;
    }

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

    public static MauiAppBuilder UseInsets(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(lifecycle =>
        {
#if ANDROID
            lifecycle.AddAndroid(androidLifecycle =>
            {
                androidLifecycle.OnApplicationCreate((application) =>
                {
                    if (application is IPlatformApplication platformApp)
                    {
                        var appInterface = platformApp.Services.GetService<IApplication>();

                        if (appInterface is Application app)
                        {
                            Insets.Current.Init(app.MainPage);
                        }
                    }
                })
                .OnPostCreate((activity, bundle) =>
                {
                    Insets.Current.InitActivity(activity);
                });
            });
#elif IOS || MACCATALYST
            lifecycle.AddiOS(iOSLifecycle =>
            {
                iOSLifecycle.FinishedLaunching((application, bundle) =>
                {
                    if (application.Delegate is IPlatformApplication platformApp)
                    {
                        var appInterface = platformApp.Services.GetService<IApplication>();

                        if (appInterface is Application app)
                        {
                            Insets.Current.Init(app.MainPage);
                        }
                    }
                    return true;
                });
            });
#elif WINDOWS
        lifecycle.AddWindows(windowsLifecycle =>
            {
                windowsLifecycle.OnLaunched((application, bundle) =>
                {
                    if (application is IPlatformApplication platformApp)
                    {
                        var appInterface = platformApp.Services.GetService<IApplication>();

                        if (appInterface is Application app)
                        {
                            Insets.Current.Init(app.MainPage);
                        }
                    }
                });
            });
#endif
        });
        return builder;
    }
}
