using Microsoft.AspNetCore.Mvc;
using SendViewBagToRazor.Models;

namespace SendViewBagToRazor.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.StringData = "Какая-то строка";
        ViewBag.LongData = 12345L;
        ViewBag.DateTimeData = DateTime.Now;
        ViewBag.RecordData = new Person("Ivan", new DateTime(1983, 9, 14), "Moscow");

        // Через View ничего не передается.
        return View();
    }
}
