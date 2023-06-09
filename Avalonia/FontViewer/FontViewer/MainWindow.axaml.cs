using Avalonia.Controls;

namespace FontViewer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void FontList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count < 1)
            return;

        var item = e.AddedItems[0];
        var font = item as string;
        if (font == null)
            return;

        if (TextPreview == null)
            return;

        // FontFamily="{Binding ElementName=FontList,Path=SelectedItem}" not worked
        var textBlock1 = TextPreview.Find<TextBlock>("TextBlock1");
        if (textBlock1 != null)
            textBlock1.FontFamily = font;

        var textBlock2 = TextPreview.FindControl<TextBlock>("TextBlock2");
        if (textBlock2 != null)
            textBlock2.FontFamily = font;

        var textBlock3 = TextPreview.FindControl<TextBlock>("TextBlock3");
        if (textBlock3 != null)
            textBlock3.FontFamily = font;

        var textBlock4 = TextPreview.FindControl<TextBlock>("TextBlock4");
        if (textBlock4 != null)
            textBlock4.FontFamily = font;
    }
}
