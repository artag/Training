namespace TextEditor;

public class Data
{
    public Data(DocumentManager documentManager, ToolbarManager toolbarManager)
    {
        DocumentManager = documentManager;
        ToolbarManager = toolbarManager;
    }

    public DocumentManager DocumentManager { get; }

    public ToolbarManager ToolbarManager { get; }

}
