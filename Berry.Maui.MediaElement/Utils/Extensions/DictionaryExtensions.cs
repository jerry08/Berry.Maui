using System.Collections;
using System.Collections.Generic;

namespace Berry.Maui.Utils.Extensions;

internal static class DictionaryExtensions
{
    public static void PutAll(
        this Dictionary<string, string> source,
        IDictionary<string, string> keyValuePairs
    )
    {
        foreach (var (key, value) in keyValuePairs)
        {
            source.Add(key, value);
        }
    }

    public static void PutAll(
        this Dictionary<string, string> source,
        IDictionary keyValuePairs
    )
    {
        foreach (DictionaryEntry entry in keyValuePairs)
        {
            var key = entry.Key?.ToString();
            var value = entry.Value?.ToString();

            if (key is null || value is null)
                continue;

            source.Add(key, value);
        }
    }
}
