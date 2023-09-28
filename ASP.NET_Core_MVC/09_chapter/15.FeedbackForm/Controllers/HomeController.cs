using Feedback.Models;
using Feedback.Services;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers;

public class HomeController : Controller
{
    private readonly IEmail _email;

    public HomeController(IEmail email)
    {
        _email = email;
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
        if (!ModelState.IsValid)
        {
            sendForm.ModelError = "Не введено одно или несколько обязательных значений";
            return RedirectToAction(nameof(Index), sendForm);
        }

        if (!sendForm.IsAssent)
        {
            sendForm.ModelError = "Не отмечено согласие на обработку данных.";
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
}
