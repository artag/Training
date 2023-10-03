using System.Net.Mime;
using System.Text;
using Feedback.Models;
using Feedback.Services;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers;

public class HomeController : Controller
{
    private const string CaptchaCodeKey = "CaptchaCode";
    private readonly IEmail _email;
    private readonly ICaptchaFactory _captchaFactory;

    public HomeController(IEmail email, ICaptchaFactory captchaFactory)
    {
        _email = email;
        _captchaFactory = captchaFactory;
    }

    public IActionResult Index(SendFormFeedback? sendForm = null)
    {
        if (string.IsNullOrEmpty(sendForm?.ModelError))
        {
            ModelState.Clear();
        }
        else
        {
            ModelState.AddModelError("", sendForm.ModelError);
        }

        return View();
    }

    [HttpPost]
    public IActionResult Send(SendFormFeedback sendForm)
    {
        var (modelValid, error) = ModelStateIsValid(sendForm);
        if (!modelValid)
        {
            sendForm.ModelError = error;
            return RedirectToAction(nameof(Index), sendForm);
        }

        var reply = $"Тема: <b>{sendForm.Subject}</b><br/><br/>" +
                    $"{sendForm.Message}<br/>" +
                    $"Отправитель: {sendForm.Name}<br/><hr>" +
                    $"<a href='mailto:{sendForm.Email}'>Ответ отправителю</a>";
        var err = _email.SendMessage(reply);
        if (err.Length > 0)
        {
            sendForm.ModelError = err;
            return RedirectToAction(nameof(Index), sendForm);
        }

        TempData["SendMessage"] = "Ваше сообщение было отправлено.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("get-captcha-image")]    // Используется для jquery обновления капчи.
    public IActionResult GetCaptchaImage()
    {
        var captchaCode = GenerateRandomCode();
        HttpContext.Session.SetString(CaptchaCodeKey, captchaCode);

        using var captchaImage = _captchaFactory.Create(captchaCode, width: 200, height: 50);
        var stream = captchaImage.Encode();
        stream.Position = 0;
        return new FileStreamResult(stream, MediaTypeNames.Image.Jpeg);
    }

    private (bool, string) ModelStateIsValid(SendFormFeedback sendForm)
    {
        if (!ModelState.IsValid)
            return (false, "Не введено одно или несколько обязательных значений.");

        var captcha = HttpContext.Session.GetString(CaptchaCodeKey);
        if (sendForm.CaptchaCode != captcha)
            return (false, "Ошибочный код: попробуйте еще раз.");

        return (true, string.Empty);
    }

    private string GenerateRandomCode()
    {
        var random = new Random();
        var sb = new StringBuilder();
        for (var i = 0; i < 6; i++)
        {
            sb.Append(random.Next(10).ToString());
        }

        return sb.ToString();
    }
}
