using Microsoft.AspNetCore.Mvc;

namespace HelloAjax.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public string Hello()
    {
        return $"<h3>Спартак отметил юбилей новым логотипом.</h3>" +
               $"<h5>{DateTime.Now.ToString("dd.MM.yyyy")}</h5>";
    }
}
