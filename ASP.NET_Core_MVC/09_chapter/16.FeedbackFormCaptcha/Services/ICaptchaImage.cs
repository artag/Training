namespace Feedback.Services;

public interface ICaptchaImage : IDisposable
{
    Stream Encode();
}
