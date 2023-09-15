using Microsoft.AspNetCore.Mvc;
using MonitorApp.Services;

namespace MonitorApp.Controllers;

public class HomeController : Controller
{
    private readonly IMonitorPanel _monitor;

    public HomeController(IMonitorPanel monitor)
    {
        _monitor = monitor;
    }

    public IActionResult Index()
    {
        var rows = _monitor.GetAll().OrderByDescending(x => x.Id).ToList();
        return View(rows);
    }
}
