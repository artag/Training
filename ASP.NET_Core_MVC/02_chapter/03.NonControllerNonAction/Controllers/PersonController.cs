using Microsoft.AspNetCore.Mvc;

namespace NonControllerNonAction.Controllers;

public class PersonController : Controller
{
    public string Name()
    {
        return "Ivanov Ivan Ivanovich";
    }

    [ActionName("email")]
    public string GetEmail()
    {
        return "ivanov@company.mail.com";
    }

    /// <summary>
    /// Игнорируемое действие.
    /// </summary>
    [NonAction]
    public ContentResult Index()
    {
        return Content($"This is controller {nameof(PersonController)}");
    }
}
