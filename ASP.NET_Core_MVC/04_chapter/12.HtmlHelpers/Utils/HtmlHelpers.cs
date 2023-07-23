using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HtmlHelpers.Utils;

public static class HtmlHelpers
{
    public static IHtmlContent TextFontStrong(this IHtmlHelper htmlHelper, string text) =>
        new HtmlString($"<strong>{text}</strong>");

    public static IHtmlContent List(this IHtmlHelper htmlHelper, params string[] rows)
    {
        var ul = new TagBuilder("ul");
        foreach (var row in rows)
        {
            var li = new TagBuilder("li");

            // Добавление текст в li
            li.InnerHtml.Append(row);

            // Добавление li в ul
            ul.InnerHtml.AppendHtml(li);
        }

        using var writer = new StringWriter();
        ul.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
        var result = writer.ToString();
        return new HtmlString(result);
    }
}
