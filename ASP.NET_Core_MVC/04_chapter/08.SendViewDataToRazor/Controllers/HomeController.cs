using Microsoft.AspNetCore.Mvc;
using SendViewDataToRazor.Models;

namespace SendViewDataToRazor.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["stringData"] = "Какая-то строка";
        ViewData["longData"] = 12345L;
        ViewData["dateTimeData"] = DateTime.Now;
        ViewData["recordData"] = new Person("Ivan", new DateTime(1983, 9, 14), "Moscow");

        // Через View ничего не передается.
        return View();
    }
}
