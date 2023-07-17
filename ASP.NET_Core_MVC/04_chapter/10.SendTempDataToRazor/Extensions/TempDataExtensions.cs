using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SendTempDataToRazor.Extensions;

public static class TempDataExtensions
{
    public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
    {
        tempData[key] = JsonSerializer.Serialize(value);
    }

    public static T? Get<T>(this ITempDataDictionary tempData, string key)
    {
        tempData.TryGetValue(key, out var o);
        return o == null ? default(T) : JsonSerializer.Deserialize<T>((string)o);
    }
}
