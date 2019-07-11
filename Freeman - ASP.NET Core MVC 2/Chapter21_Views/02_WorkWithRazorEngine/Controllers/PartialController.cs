using Microsoft.AspNetCore.Mvc;

namespace WorkWithRazorEngine.Controllers
{
    public class PartialController : Controller
    {
        public IActionResult Index() => View();
    }
}