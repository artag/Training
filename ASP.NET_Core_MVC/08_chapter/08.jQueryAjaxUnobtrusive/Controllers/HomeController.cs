using AjaxUnobtrusive.Models;
using Microsoft.AspNetCore.Mvc;

namespace AjaxUnobtrusive.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SendData(Person person)
    {
        // Имитация выполнения некоторой работы с данными.
        Thread.Sleep(3000);

        if (person.TestError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Json(person);
    }
}
