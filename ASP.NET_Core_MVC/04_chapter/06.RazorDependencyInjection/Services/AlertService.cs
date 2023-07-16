namespace DependencyInjectionInView.Services;

public class AlertService : IAlert
{
    public string GetMessage()
    {
        return "Внимание, важное сообщение!";
    }
}
