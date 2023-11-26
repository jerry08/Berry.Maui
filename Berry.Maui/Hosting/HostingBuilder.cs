using Berry.Maui.Behaviors;
using Berry.Maui.Controls;
using Berry.Maui.Controls.Effects;
using Berry.Maui.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace Berry.Maui;

public class HostingBuilder
{
    private readonly MauiAppBuilder _builder;

    public HostingBuilder(MauiAppBuilder builder)
    {
        _builder = builder;
        ConfigureWorkarounds();
    }

    internal HostingBuilder ConfigureWorkarounds()
    {
        _builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            handlers.AddHandler(typeof(Page), typeof(WorkaroundPageHandler));
#endif
        });
        return this;
    }

    public HostingBuilder UseMaterialEntry()
    {
        _builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            handlers.AddHandler(typeof(MaterialEntry), typeof(MaterialEntryHandler));
#endif
        });
        return this;
    }

    public HostingBuilder UseBottomSheet()
    {
        _builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<BottomSheet, BottomSheetHandler>();
        });
        return this;
    }

    public HostingBuilder UseAcrylicView()
    {
        _builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(AcrylicView), typeof(AcrylicViewHandler));
        });
        return this;
    }

    public HostingBuilder UseMauiPlainer()
    {
        _builder.ConfigureMauiHandlers(handlers => handlers.AddPlainer());
        return this;
    }

    public HostingBuilder UseTabs()
    {
        _builder.ConfigureEffects(x =>
        {
#if ANDROID
            x.Add<CommandsRoutingEffect, Berry.Maui.Controls.Effects.Droid.CommandsPlatform>();
            x.Add<TouchRoutingEffect, Berry.Maui.Controls.Effects.Droid.TouchEffectPlatform>();
            x.Add<TintableImageEffect, Berry.Maui.Controls.Droid.AndroidTintableImageEffect>();
#elif IOS
            x.Add<CommandsRoutingEffect, Berry.Maui.Controls.Effects.iOS.CommandsPlatform>();
            x.Add<TouchRoutingEffect, Berry.Maui.Controls.Effects.iOS.TouchEffectPlatform>();
            x.Add<TintableImageEffect, Berry.Maui.Controls.iOS.iOSTintableImageEffect>();
#endif
        });

        return this;
    }

    public HostingBuilder UseInsets()
    {
        _builder.ConfigureLifecycleEvents(lifecycle =>
        {
#if ANDROID
            lifecycle.AddAndroid(androidLifecycle =>
            {
                androidLifecycle
                    .OnApplicationCreate(
                        (application) =>
                        {
                            if (application is IPlatformApplication platformApp)
                            {
                                var appInterface = platformApp.Services.GetService<IApplication>();

                                if (appInterface is Application app)
                                {
                                    Insets.Current.Init(app.MainPage);
                                }
                            }
                        }
                    )
                    .OnPostCreate(
                        (activity, bundle) =>
                        {
                            Insets.Current.InitActivity(activity);
                        }
                    );
            });
#elif IOS || MACCATALYST
            lifecycle.AddiOS(iOSLifecycle =>
            {
                iOSLifecycle.FinishedLaunching(
                    (application, bundle) =>
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
                    }
                );
            });
#elif WINDOWS
            lifecycle.AddWindows(windowsLifecycle =>
            {
                windowsLifecycle.OnLaunched(
                    (application, bundle) =>
                    {
                        if (application is IPlatformApplication platformApp)
                        {
                            var appInterface = platformApp.Services.GetService<IApplication>();

                            if (appInterface is Application app)
                            {
                                Insets.Current.Init(app.MainPage);
                            }
                        }
                    }
                );
            });
#endif
        });

        return this;
    }

    public HostingBuilder UseMaterialSwitch(bool applyToAll = true)
    {
#if ANDROID && NET7_0_OR_GREATER
        _builder.ConfigureMauiHandlers(handlers =>
        {
            if (applyToAll)
            {
                handlers.AddHandler<Switch, MaterialSwitchHandler>();
            }
            else
            {
                handlers.AddHandler<MaterialSwitch, MaterialSwitchHandler>();
            }
        });
#endif

        return this;
    }
}
