using Microsoft.AspNetCore.Mvc;

namespace UnderstandingFilters.Controllers
{
    // Атрибут фильтра действует на все методы контроллера
    [RequireHttps]
    public class HomeController : Controller
    {
        // Но можно ставить атрибут фильтра отдельно, для каждого метода.
        // [RequireHttps]
        public IActionResult Index() =>
            View("Message", "This is the Index action on the Home controller");

        public IActionResult SecondAction() =>
            View("Message", "This is the Second action on the Home controller");

    }
}
