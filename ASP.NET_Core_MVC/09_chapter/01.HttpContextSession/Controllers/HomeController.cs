using System.Text.Json;
using HttpContextSession.Models;
using Microsoft.AspNetCore.Mvc;

namespace HttpContextSession.Controllers;

public class HomeController : Controller
{
    private const string SessionName = "_Name";
    private const string SessionAge = "_Age";
    private const string SessionPerson = "_Person";

    public IActionResult Index()
    {
        ViewData["Title"] = "Сессия";

        HttpContext.Session.SetString(SessionName, "Stan");
        HttpContext.Session.SetInt32(SessionAge, 50);

        var person = new Person { Name = "Ivan", Age = 33 };
        var json = JsonSerializer.Serialize<Person>(person);
        HttpContext.Session.SetString(SessionPerson, json);

        return View();
    }

    public IActionResult About()
    {
        ViewBag.Name = HttpContext.Session.GetString(SessionName);
        ViewBag.Age = HttpContext.Session.GetInt32(SessionAge);
        ViewData["Title"] = "Обо мне";

        return View();
    }

    public IActionResult Person()
    {
        var value = HttpContext.Session.GetString(SessionPerson);
        var person = string.IsNullOrEmpty(value)
            ? default(Person)
            : JsonSerializer.Deserialize<Person>(value);
        ViewData["Title"] = "Субъект";

        return View(person);
    }
}
