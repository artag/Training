namespace Feedback.Services;

public interface ICaptchaFactory
{
     ICaptchaImage Create(string text, int width, int height);
}
