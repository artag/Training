using System;
using Avalonia.Controls;

namespace TextEditor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var textBoxFont = new TextBoxFont(Body);
        var data = new Data(
            new DocumentManager(Body, StatusBar, this),
            new ToolbarManager(textBoxFont));
        DataContext = data;

    }

    private void MainWindow_OnDataContextChanged(object? sender, EventArgs e)
    {
        if (StatusBar == null)
            return;
        StatusBar.Text = "Ready.";
    }
}
