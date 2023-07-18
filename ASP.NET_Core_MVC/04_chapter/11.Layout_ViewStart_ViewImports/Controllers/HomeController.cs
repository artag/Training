using Microsoft.AspNetCore.Mvc;

namespace Layout_ViewStart_ViewImports.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
