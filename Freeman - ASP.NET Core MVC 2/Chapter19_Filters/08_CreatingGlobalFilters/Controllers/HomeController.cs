using Microsoft.AspNetCore.Mvc;

namespace CreatingGlobalFilters.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() =>
            View("Message", "This is the Index action on the Home controller");
    }
}
