using Avalonia.Controls;

namespace TextEditor;

public class Data
{
    public Data(DocumentManager documentManager)
    {
        DocumentManager = documentManager;
    }

    public DocumentManager DocumentManager { get; }
}
