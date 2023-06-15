using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TextEditor;

public partial class TextEditorToolbar : UserControl
{
    private Data? _data;

    public TextEditorToolbar()
    {
        InitializeComponent();
    }

    private Data Data
    {
        get
        {
            if (_data != null)
                return _data;

            var data = (Data)DataContext!;
            return _data = data;
        }
    }

    private void TextEditorToolbar_OnInitialized(object? sender, EventArgs e)
    {
        var sizes = new List<double>();
        for (double i = 8; i < 48; i += 2)
        {
            sizes.Add(i);
        }

        FontSizes.Items = sizes;
        FontSizes.SelectedIndex = 0;
        Fonts.SelectedIndex = 0;
    }

    private void OpenButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var documentManager = Data.DocumentManager;
        var openTask = documentManager.OpenDocument();
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var documentManager = Data.DocumentManager;
        var saveTask = documentManager.SaveDocument();
    }
}
