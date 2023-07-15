using Microsoft.AspNetCore.Mvc;

namespace RazorTryCatchFinally.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
