using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Utils;

public static class FilterContextExtensions
{
    public static async Task<string> ReadFromRequest(this FilterContext context)
    {
        var body = context.HttpContext.Request.Body;
        using var reader = new StreamReader(body);
        return await reader.ReadToEndAsync();
    }

    public static void SaveToRequest(this FilterContext context, string input, string added)
    {
        var bytes = TextToBytes(input, added);
        context.HttpContext.Request.Body = new MemoryStream(bytes);
    }

    private static byte[] TextToBytes(string input, string added)
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrEmpty(input))
        {
            sb.Append(input);
            sb.Append(", ");
        }

        sb.Append(added);
        var modified = sb.ToString();
        var bytes = Encoding.UTF8.GetBytes(modified);
        return bytes;
    }
}
