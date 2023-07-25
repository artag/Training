using Microsoft.AspNetCore.Mvc;

namespace BlazorServer.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
