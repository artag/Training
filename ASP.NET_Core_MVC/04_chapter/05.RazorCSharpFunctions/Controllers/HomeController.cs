using Microsoft.AspNetCore.Mvc;

namespace RazorCSharpFunctions.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
