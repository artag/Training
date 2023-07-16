using Microsoft.AspNetCore.Mvc;

namespace DependencyInjectionInView.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}