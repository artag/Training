using Microsoft.AspNetCore.Mvc;

namespace ViewComponentExample.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Компонент представления (View Component)";
        return View();
    }
}
