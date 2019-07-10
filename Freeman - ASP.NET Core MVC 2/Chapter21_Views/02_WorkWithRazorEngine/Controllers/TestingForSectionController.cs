using Microsoft.AspNetCore.Mvc;

namespace WorkWithRazorEngine.Controllers
{
    public class TestingForSectionController : Controller
    {
        public IActionResult Index() => View(new[] { "Apple", "Orange", "Pear" });
    }
}