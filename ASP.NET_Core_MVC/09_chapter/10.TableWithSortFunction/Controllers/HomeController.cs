using Microsoft.AspNetCore.Mvc;
using TableSort.Infrastructure;
using TableSort.Models;
using TableSort.Services;

namespace TableSort.Controllers;

public class HomeController : Controller
{
    private readonly IPersons _persons;

    public HomeController(IPersons persons)
    {
        _persons = persons;
    }

    public IActionResult Index(SortStatus orderBy = SortStatus.IdAsc)
    {
        var people = _persons.GetAll() ?? Array.Empty<Person>();
        ViewBag.IdSort = orderBy == SortStatus.IdAsc
            ? SortStatus.IdDesc
            : SortStatus.IdAsc;

        ViewBag.FirstNameSort = orderBy == SortStatus.FirstNameAsc
            ? SortStatus.FirstNameDesc
            : SortStatus.FirstNameAsc;

        ViewBag.LastNameSort = orderBy == SortStatus.LastNameAsc
            ? SortStatus.LastNameDesc
            : SortStatus.LastNameAsc;

        ViewBag.BirthDateSort = orderBy == SortStatus.BirthDateAsc
            ? SortStatus.BirthDateDesc
            : SortStatus.BirthDateAsc;

        var sorted = orderBy switch
        {
            SortStatus.IdDesc => people.OrderByDescending(p => p.Id),
            SortStatus.FirstNameAsc => people.OrderBy(p => p.FirstName),
            SortStatus.FirstNameDesc => people.OrderByDescending(p => p.FirstName),
            SortStatus.LastNameAsc => people.OrderBy(p => p.LastName),
            SortStatus.LastNameDesc => people.OrderByDescending(p => p.LastName),
            SortStatus.BirthDateAsc => people.OrderBy(p => p.BirthDate),
            SortStatus.BirthDateDesc => people.OrderByDescending(p => p.BirthDate),
            _ => people.OrderBy(p => p.Id)
        };

        var viewModel = new PersonView
        {
            Persons = sorted,
            SortStatus = orderBy,
        };

        return View(viewModel);
    }
}
