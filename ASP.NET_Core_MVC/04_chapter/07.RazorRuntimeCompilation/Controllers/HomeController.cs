using Microsoft.AspNetCore.Mvc;

namespace RazorRuntimeCompilation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}