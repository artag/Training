namespace Layout_ViewStart_ViewImports.Services;

public class MessageService : IMessage
{
    public string GetMessage()
    {
        return $"Hello from {nameof(MessageService)}!";
    }
}
