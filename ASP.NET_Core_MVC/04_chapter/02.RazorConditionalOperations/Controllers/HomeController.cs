using Microsoft.AspNetCore.Mvc;

namespace RazorConditionalOperations.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
