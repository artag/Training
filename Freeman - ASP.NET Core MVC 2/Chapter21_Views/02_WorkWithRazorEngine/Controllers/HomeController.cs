using Microsoft.AspNetCore.Mvc;

namespace WorkWithRazorEngine.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
