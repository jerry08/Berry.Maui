using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;

namespace Berry.Maui.Hosting;

public static class ServiceProviderExtensions
{
    public static ILogger<T>? CreateLogger<T>(this IMauiContext context) =>
        context.Services.CreateLogger<T>();

    public static ILogger<T>? CreateLogger<T>(this IServiceProvider services) =>
        services.GetService<ILogger<T>>();

    public static ILogger? CreateLogger(this IMauiContext context, string loggerName) =>
        context.Services.CreateLogger(loggerName);

    public static ILogger? CreateLogger(this IServiceProvider services, string loggerName) =>
        services.GetService<ILoggerFactory>()?.CreateLogger(loggerName);
}
