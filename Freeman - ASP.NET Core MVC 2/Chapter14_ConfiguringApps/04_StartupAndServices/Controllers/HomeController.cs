using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StartupAndServices.Infrastructure;

namespace StartupAndServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly UptimeService _uptimeService;

        public HomeController(UptimeService uptimeService)
        {
            _uptimeService = uptimeService;
        }

        public ViewResult Index()
        {
            var model = new Dictionary<string, string>
            {
                ["Message"] = "This is the Index action",
                ["Uptime"] = $"{_uptimeService.Uptime}ms"
            };

            return View(model);
        }
    }
}
