using Microsoft.Maui.Controls;

namespace Berry.Maui.Extensions;

public static class ResourcesExtension
{
    public static T? GetResourceOrDefault<T>(this Application app, string key, T? defaultValue)
    {
        if (app.Resources.TryGetValue(key, out var value))
        {
            return (T?)value;
        }
        return defaultValue;
    }
}
