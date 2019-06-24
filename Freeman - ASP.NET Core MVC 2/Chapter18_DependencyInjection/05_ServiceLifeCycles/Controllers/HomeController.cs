using Microsoft.AspNetCore.Mvc;

namespace ServiceLifeCycles.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
