using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace TextEditor;

public class DocumentManager
{
    private readonly TextBox _editor;
    private readonly TextBlock _statusBar;
    private readonly Window _parent;
    private string? _currentFile;


    public DocumentManager(
        TextBox editor,
        TextBlock statusBar,
        Window parent)
    {
        _editor = editor;
        _statusBar = statusBar;
        _parent = parent;
    }

    public async Task OpenDocument()
    {
        var dlg = new OpenFileDialog();
        dlg.AllowMultiple = false;
        dlg.Filters = GetFileFilters();
        var files = await dlg.ShowAsync(_parent);
        _currentFile = files?.FirstOrDefault();
        if (_currentFile == null)
            return;

        var txt = await File.ReadAllTextAsync(_currentFile);
        _editor.Text = txt;
        _statusBar.Text = $"Text successfully loaded from the file {_currentFile}.";
    }

    public async Task SaveDocument()
    {
        var text = _editor.Text;
        if (string.IsNullOrWhiteSpace(text))
        {
            _statusBar.Text = "Nothing to save. Cancelling save operation.";
            return;
        }

        var dlg = new SaveFileDialog();
        dlg.DefaultExtension = "txt";
        dlg.Filters = GetFileFilters();
        _currentFile = await dlg.ShowAsync(_parent);
        if (_currentFile == null)
        {
            _statusBar.Text = $"File to save is not selected. Cancelling save operation.";
            return;
        }

        await File.WriteAllTextAsync(_currentFile, _editor.Text);
        _statusBar.Text = $"Text successfully saved to the file {_currentFile}.";
    }

    private static List<FileDialogFilter> GetFileFilters() =>
        new List<FileDialogFilter>()
        {
            new FileDialogFilter() { Extensions = new List<string>() { "txt" }, Name = "Text Document (.txt)" },
            new FileDialogFilter() { Extensions = new List<string>() { "*" }, Name = "All files" },
        };
}
