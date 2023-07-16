namespace RazorRuntimeCompilation.Services;

public class AlertService : IAlert
{
    public string GetMessage()
    {
        return "Внимание! Очень важное сообщение.";
    }
}