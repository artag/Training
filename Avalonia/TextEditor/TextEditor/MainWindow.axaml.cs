using System;
using Avalonia.Controls;

namespace TextEditor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var data = new Data(new DocumentManager(Body, StatusBar, this));
        DataContext = data;
    }

    private void MainWindow_OnDataContextChanged(object? sender, EventArgs e)
    {
        if (StatusBar == null)
            return;
        StatusBar.Text = "Ready.";
    }
}