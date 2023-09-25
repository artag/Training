using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using TableCrud.Extensions;
using TableCrud.Models;
using TableCrud.Services;

namespace TableCrud.Controllers;

public class HomeController : Controller
{
    private readonly ICountries _countries;

    public HomeController(ICountries countries)
    {
        _countries = countries;
    }

    [HttpGet]
    public IActionResult Index(int rowUpdate = 0)
    {
        var errors = TempData.GetModelStateError();
        if (errors.Any())
            foreach (var error in errors)
                ViewData.ModelState.AddModelError("", error);

        ViewBag.RowUpdate = rowUpdate;
        return View(GetAllOrderedCountries());
    }

    [HttpPost]
    public IActionResult Edit(string countryName, int saveId = 0)
    {
        if (saveId > 0)
        {
            try
            {
                var newCountry = new Country { Id = saveId, Name = countryName };
                _countries.Edit(newCountry);
            }
            catch (Exception ex)
            {
                TempData.PutModelStateError(ex.Message);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        TempData.PutModelStateError("Выберите запись, чтобы сохранить изменение.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(int deleteId = 0, int saveId = 0)
    {
        try
        {
            var country = _countries
                .Get()
                .FirstOrDefault(c => c.Id == deleteId);
            if (country != null)
            {
                _countries.Delete(country);
                TempData["messageDeleteInfo"] = $"Запись о стране {country.Name} успешно удалена.";
            }
        }
        catch (Exception exc)
        {
            TempData["messageDeleteError"] = exc.Message;
        }

        return saveId == 0 || deleteId == saveId
            ? RedirectToAction(nameof(Index), "Home")
            : RedirectToAction(nameof(Index), "Home", new { rowUpdate = saveId });
    }

    [HttpPost]
    public IActionResult Add(string countryName)
    {
        if (!string.IsNullOrWhiteSpace(countryName))
        {
            try
            {
                var country = new Country
                {
                    Id = 0,
                    Name = countryName,
                };
                _countries.Add(country);
            }
            catch (Exception ex)
            {
                TempData.PutModelStateError(ex.Message);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        TempData.PutModelStateError("Введите наименование страны.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult CancelUpdate()
    {
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpPost]
    public IActionResult Update(int updateId = 0)
    {
        return RedirectToAction(nameof(Index), "Home", new { rowUpdate = updateId });
    }

    private ImmutableArray<Country> GetAllOrderedCountries() =>
        _countries
            .Get()
            .OrderByDescending(x => x.Id)
            .ToImmutableArray();
}
