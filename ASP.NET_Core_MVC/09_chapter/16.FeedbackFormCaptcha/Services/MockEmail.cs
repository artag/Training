namespace Feedback.Services;

/// <summary>
/// Эмуляция посылки сообщения (всегда успешно).
/// </summary>
public class MockEmail : IEmail
{
    public string SendMessage(string reply)
    {
        return string.Empty;
    }
}
