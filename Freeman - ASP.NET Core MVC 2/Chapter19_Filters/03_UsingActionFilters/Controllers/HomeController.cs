using Microsoft.AspNetCore.Mvc;
using UsingActionFilters.Infrastructure;

namespace UsingActionFilters.Controllers
{
    public class HomeController : Controller
    {
        [Profile]
        public ViewResult Index() =>
            View("Message", "This is the Index action on the Home controller");

        [ProfileAsync]
        public ViewResult SecondAction() =>
            View("Message", "This is the Second action on the Home controller");
    }
}
