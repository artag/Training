using Microsoft.AspNetCore.Mvc;
using TableGroup.Services;

namespace TableGroup.Controllers;

public class HomeController : Controller
{
    private readonly IOffices _offices;

    public HomeController(IOffices offices)
    {
        _offices = offices;
    }

    public IActionResult Index()
    {
        return View(_offices.GetAll());
    }
}
