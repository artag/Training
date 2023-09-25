using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TableCrud.Extensions;

public static class TempDataExtensions
{
    private const string ModelStateError = "ModelStateError";

    public static void PutModelStateError(this ITempDataDictionary tempData, params string[] errors) =>
        Put(tempData, ModelStateError, errors);

    public static string[] GetModelStateError(this ITempDataDictionary tempData) =>
        Get<string[]>(tempData, ModelStateError) ?? Array.Empty<string>();

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
