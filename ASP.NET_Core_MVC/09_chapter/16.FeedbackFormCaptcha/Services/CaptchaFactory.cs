namespace Feedback.Services;

public class CaptchaFactory : ICaptchaFactory
{
    public ICaptchaImage Create(string text, int width, int height) =>
        new CaptchaImage(text, width, height);
}
