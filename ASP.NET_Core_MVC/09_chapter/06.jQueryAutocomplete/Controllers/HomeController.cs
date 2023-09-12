using Autocomplete.Models;
using Autocomplete.Services;
using Microsoft.AspNetCore.Mvc;

namespace Autocomplete.Controllers;

public class HomeController : Controller
{
    private readonly IPersons _persons;

    public HomeController(IPersons persons)
    {
        _persons = persons;
    }

    /// <summary>
    /// Отображает представление.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Подгружает данные в список автоподстановки.
    /// </summary>
    [HttpGet]
    public IActionResult Find()
    {
        var name = HttpContext.Request.Query["fragment"].ToString();
        var data = _persons.People
            .Where(p => p.Fio.ToLower().Contains(name.ToLower()))
            .ToArray();

        return Ok(data);
    }

    /// <summary>
    /// Обрабатывает POST-запросы.<br/>
    /// Осуществляет динамический поиск при нажатии на кнопку 'Найти'.
    /// </summary>
    [HttpPost]
    public IActionResult Search(string search)
    {
        return RedirectToAction(nameof(Index), "Home", new { search = search });
    }
}
