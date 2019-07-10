using Microsoft.AspNetCore.Mvc;

namespace WorkWithRazorEngine.Controllers
{
    public class AllSectionController : Controller
    {
        public IActionResult Index() => View(new[] { "Apple", "Orange", "Pear" });
    }
}