using Microsoft.AspNetCore.Mvc;

namespace DataPickerExample.Controllers;

public class HomeController : Controller
{
    [HttpGet()]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost()]
    public IActionResult Index(string selectedDate)
    {
        ViewBag.Message = "Выбрана дата: " + selectedDate;
        return View();
    }
}
