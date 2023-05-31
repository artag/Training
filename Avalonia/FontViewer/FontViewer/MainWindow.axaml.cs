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

        if (TextBlock1 == null)
            return;

        // FontFamily="{Binding ElementName=FontList,Path=SelectedItem}" not worked
        TextBlock1.FontFamily = font;
        TextBlock2.FontFamily = font;
        TextBlock3.FontFamily = font;
        TextBlock4.FontFamily = font;
    }
}
