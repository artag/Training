using Microsoft.AspNetCore.Mvc;
using TablePage.Models;
using TablePage.Services;

namespace TablePage.Controllers;

public class HomeController : Controller
{
    /// <summary>
    /// Количество записей на странице.
    /// </summary>
    private const int PageSize = 5;
    private readonly IPersons _persons;

    public HomeController(IPersons persons)
    {
        _persons = persons;
    }

    public IActionResult Index(int? page)
    {
        var count = _persons.GetAll().Length;
        var p = new PagingInfo
        {
            CurrentPage = page ?? 1,
            ItemsPerPage = PageSize,
            TotalItems = count
        };

        ViewBag.PageInfo = p;
        var query = _persons
            .GetAll()
            .OrderBy(p => p.Id)
            .Skip((p.CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToArray();

        return View(query);
    }
}
