using Dropdownlist.Models;
using Dropdownlist.Services;
using Dropdownlist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dropdownlist.Controllers;

public class HomeController : Controller
{
    private readonly IRegions _regions;

    public HomeController(IRegions regions)
    {
        _regions = regions;
    }

    public IActionResult Index(int countryId = 0)
    {
        ViewBag.Countries = GetCountryForSelect();
        var cities = _regions.Cities;
        if (countryId > 0)
            cities = cities.Where(c => c.CountryId == countryId).ToList();

        return View(new CountryView { CountryId = countryId, Cities = cities });
    }

    // ВАЖНО. Наименование параметра countryId должно соответствовать имени элемента <select> во View.
    [HttpPost]
    public IActionResult Select(int countryId)
    {
        return RedirectToAction("Index", "Home", new { countryId = countryId });
    }

    private SelectList GetCountryForSelect()
    {
        var countries = _regions.Countries;
        var selection = countries.Where(c => c.Id == 0);
        if (!selection.Any())
            countries.Add(new Country { Id = 0, Name = "Все страны" });

        var ordered = countries.OrderBy(c => c.Id);
        return new SelectList(ordered, "Id", "Name");
    }
}
