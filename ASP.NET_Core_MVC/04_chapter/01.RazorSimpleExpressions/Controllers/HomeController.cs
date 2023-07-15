using Microsoft.AspNetCore.Mvc;

namespace RazorSimpleExpressions.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}