using Microsoft.AspNetCore.Mvc;
using ManagingFilterLifeCycles.Infrastructure;

namespace ManagingFilterLifeCycles.Controllers
{
    [TypeFilter(typeof(DiagnosticsFilter))]
    [ServiceFilter(typeof(TimeFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index() =>
            View("Message", "This is the Index action on the Home controller");
    }
}
