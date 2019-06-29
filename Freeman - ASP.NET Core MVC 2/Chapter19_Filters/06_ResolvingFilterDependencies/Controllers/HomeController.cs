using Microsoft.AspNetCore.Mvc;
using ResolvingFilterDependencies.Infrastructure;

namespace ResolvingFilterDependencies.Controllers
{
    [TypeFilter(typeof(DiagnosticsFilter))]
    [TypeFilter(typeof(TimeFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index() =>
            View("Message", "This is the Index action on the Home controller");
    }
}
