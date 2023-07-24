using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpers.Tags;

[HtmlTargetElement("hello", TagStructure = TagStructure.NormalOrSelfClosing)]
public class HelloTag : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = string.Empty;
        var content =
            $"<h3>" +
            $"Привет, мир. " +
            $"Мое время: {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}" +
            $"</h3>";
        output.Content.SetHtmlContent(content);
    }
}
