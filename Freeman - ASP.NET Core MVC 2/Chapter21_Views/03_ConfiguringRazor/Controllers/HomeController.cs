using Microsoft.AspNetCore.Mvc;

namespace ConfiguringRazor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}