using Microsoft.AspNetCore.Mvc;

namespace WorkWithRazorEngine.Views.Home
{
    public class SectionController : Controller
    {
        public IActionResult Index() => View(new[] { "Apple", "Orange", "Pear" });
    }
}