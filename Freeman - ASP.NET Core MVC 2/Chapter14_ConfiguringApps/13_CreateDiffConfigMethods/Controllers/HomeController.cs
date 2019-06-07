using CreateDiffConfigMethods.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CreateDiffConfigMethods.Controllers
{
    public class HomeController : Controller
    {
        private readonly UptimeService _uptimeService;

        public HomeController(UptimeService _uptimeService)
        {
            this._uptimeService = _uptimeService;
        }

        public IActionResult Index()
        {
            var message = _uptimeService.DisplayUptime;
            return View((object)message);
        }
    }
}
