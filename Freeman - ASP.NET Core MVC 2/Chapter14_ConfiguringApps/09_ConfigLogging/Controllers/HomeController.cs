using System.Collections.Generic;
using ConfigLogging.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConfigLogging.Controllers
{
    public class HomeController : Controller
    {
        private readonly UptimeService _uptimeService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UptimeService uptimeService, ILogger<HomeController> logger)
        {
            _uptimeService = uptimeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogDebug($"Handled {Request.Path} at uptime {_uptimeService.Uptime}");

            var model = new Dictionary<string, string>
            {
                ["Message"] = "This is the Index action",
                ["Uptime"] = $"{_uptimeService.Uptime}ms"
            };

            return View(model);
        }
    }
}
