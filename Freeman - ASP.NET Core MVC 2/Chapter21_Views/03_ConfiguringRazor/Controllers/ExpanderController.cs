using Microsoft.AspNetCore.Mvc;

namespace ConfiguringRazor.Controllers
{
    public class ExpanderController : Controller
    {
        // Путь в никуда.
        public IActionResult Index() => View("MyView");
    }
}