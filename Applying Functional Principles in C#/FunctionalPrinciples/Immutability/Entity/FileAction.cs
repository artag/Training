namespace Immutability
{
    public struct FileAction
    {
        public FileAction(string fileName, ActionType type, string[] content)
        {
            FileName = fileName;
            Type = type;
            Content = content;
        }

        public string FileName { get; }
        public ActionType Type { get; }
        public string[] Content { get; }
    }
}