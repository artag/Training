using Microsoft.AspNetCore.Mvc;
using UsingResultFilters.Infrastructure;

namespace UsingResultFilters.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [ViewResultDetails]
        public IActionResult FirstAction() =>
            View("Message", "This is the Index action on the Home controller");

        [ViewResultDetailsAsync]
        public IActionResult SecondAction() =>
            View("Message", "This is the Second action on the Home controller");

        [Profile]
        public IActionResult ThirdAction() =>
            View("Message", "This is the Third action on the Home controller");
    }
}
