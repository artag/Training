using Microsoft.AspNetCore.Mvc;

namespace RazorLoops.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
