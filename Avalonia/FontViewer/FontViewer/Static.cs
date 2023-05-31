using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace FontViewer;

public class Static
{
    private static readonly SKFontManager _fontManager = SKFontManager.Default;
    public static ICollection<string> Fonts =>
        _fontManager.FontFamilies
            .OrderBy(f => f)
            .ToArray();
}
