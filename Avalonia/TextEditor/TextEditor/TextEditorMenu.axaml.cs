using Avalonia.Controls;
using Avalonia.Interactivity;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace TextEditor;

public partial class TextEditorMenu : UserControl
{
    public TextEditorMenu()
    {
        InitializeComponent();
    }

    private void About_OnClick(object? sender, RoutedEventArgs e)
    {
        var messageBox = MessageBoxManager.GetMessageBoxStandardWindow(
            title: "About",
            text: "   Teach Yourself WPF in 24 Hours - Text Editor   ",
            ButtonEnum.Ok);

        messageBox.Show();
    }
}
