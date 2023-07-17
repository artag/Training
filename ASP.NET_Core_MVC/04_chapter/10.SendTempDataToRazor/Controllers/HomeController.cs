using Microsoft.AspNetCore.Mvc;
using SendTempDataToRazor.Extensions;
using SendViewBagToRazor.Models;

namespace SendTempDataToRazor.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        TempData["StringData"] = "Какая-то строка";
        TempData["DateTimeData"] = DateTime.Now;

        // Используется вспомогательный метод расширения для сериализации значений.
        TempData.Put("LongData", 12345L);

        var person = new Person("Ivan", new DateTime(1983, 9, 14), "Moscow");
        TempData.Put("RecordData", person);

        // Через View ничего не передается.
        return View();
    }

    public IActionResult Default()
    {
        return View();
    }

    public IActionResult Next()
    {
        return View();
    }
}
