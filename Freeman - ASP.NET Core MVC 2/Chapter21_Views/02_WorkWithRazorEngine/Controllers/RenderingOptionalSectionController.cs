using Microsoft.AspNetCore.Mvc;

namespace WorkWithRazorEngine.Controllers
{
    public class RenderingOptionalSectionController : Controller
    {
        public IActionResult Index() => View(new[] { "Apple", "Orange", "Pear" });
    }
}