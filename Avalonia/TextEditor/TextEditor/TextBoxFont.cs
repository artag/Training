using Avalonia.Controls;

namespace TextEditor;

public interface IFont
{
    void SetFont(string font);
}

public class TextBoxFont : IFont
{
    private readonly TextBox _textBox;

    public TextBoxFont(TextBox textBox)
    {
        _textBox = textBox;
    }

    public void SetFont(string font)
    {
        _textBox.FontFamily = font;
    }
}
