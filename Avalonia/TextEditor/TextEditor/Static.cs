using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace TextEditor;

public class Static
{
    private static readonly SKFontManager _fontManager = SKFontManager.Default;
    public static ICollection<string> Fonts =>
        _fontManager.FontFamilies
            .OrderBy(f => f)
            .ToArray();
}
