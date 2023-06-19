using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;

namespace TextEditor;

public class ToolbarManager : INotifyPropertyChanged
{
    private readonly IFont _font;
    private double _fontSize = 8;

    public ToolbarManager(IFont font)
    {
        _font = font;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public IFont Font => _font;

    public double FontSize
    {
        get => _fontSize;
        set
        {
            if (Math.Abs(_fontSize - value) < double.Epsilon)
                return;

            _fontSize = value;
            OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
