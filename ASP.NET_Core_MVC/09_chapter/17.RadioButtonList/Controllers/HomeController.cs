using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RadioButtonList.Models;
using RadioButtonList.Services;

namespace RadioButtonList.Controllers;

public class HomeController : Controller
{
    private readonly IFruits _fruits;

    public HomeController(IFruits fruits)
    {
        _fruits = fruits;
    }

    public IActionResult Index()
    {
        var items = GetFruits();
        return View(items);
    }

    [HttpPost]
    public IActionResult Choose(string fruit)
    {
        var items = GetFruits();
        var selectedItem = items.FirstOrDefault(i => i.Value == fruit);
        if (selectedItem != null)
        {
            selectedItem.Selected = true;
            ViewBag.Message = "Выбрано: " + selectedItem.Text;
        }

        return View(nameof(Index), items);
    }

    private SelectListItem[] GetFruits()
    {
        var fruits = _fruits.Get() ?? Array.Empty<Fruit>();
        return fruits
            .Select(fruit => new SelectListItem
            {
                Text = fruit.Name,
                Value = fruit.Id.ToString()
            })
            .ToArray();
    }
}