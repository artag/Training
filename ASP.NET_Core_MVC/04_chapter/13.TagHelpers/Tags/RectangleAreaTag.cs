using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpers.Tags;

[HtmlTargetElement("rectangle-area", TagStructure = TagStructure.NormalOrSelfClosing)]
public class RectangleAreaTag : TagHelper
{
    [HtmlAttributeName("a")]
    public double A { get; set; }

    [HtmlAttributeName("b")]
    public double B { get; set; }

    [HtmlAttributeName("units")]
    public Units Units { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var area = A * B;

        var units = UnitsToString();
        var text = $"Площадь прямоугольника " +
                   $"{A.ToString()} &times; {B.ToString()}: " +
                   $"{area.ToString()} {units} <sup>2</sup>";

        output.TagName = string.Empty;
        output.Content.SetHtmlContent(text);
        return Task.CompletedTask;
    }

    private string UnitsToString()
    {
        switch (Units)
        {
            case Units.Mm:
                return "мм";
            case Units.Cm:
                return "см";
            case Units.M:
                return "м";
            case Units.Km:
                return "км";
            default:
                return string.Empty;
        }
    }
}

public enum Units
{
    Mm,
    Cm,
    M,
    Km,
}
