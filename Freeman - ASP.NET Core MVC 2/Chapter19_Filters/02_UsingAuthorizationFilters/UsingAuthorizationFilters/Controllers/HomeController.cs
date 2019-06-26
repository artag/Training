using Microsoft.AspNetCore.Mvc;
using UsingAuthorizationFilters.Infrastructure;

namespace UsingAuthorizationFilters.Controllers
{
    [HttpsOnly]
    public class HomeController : Controller
    {
        public IActionResult Index() =>
            View("Message", "This is the Index action on the Home controller");
    }
}
