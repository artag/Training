using FiltersSample.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FiltersSample.Controllers;

public class HomeController : Controller
{
    [BrowserFilter]
    [OrderedFilter(Order = 1)]
    [AdminFilter("user")] // или [AdminFilter("admin")]

    // ServiceFilter извлекает объект фильтра и его зависимости из DI.
    // Требует регистрации ConfigFilter в DI.
    [ServiceFilter(typeof(ConfigFilter))]

    // Или, вместо ServiceFilter.
    // TypeFilter извлекает все зависимости фильтра через DI, сам фильтр конструирует через ObjectFactory.
    // Не требует регистрации ConfigFilter в DI.
    // [TypeFilter(typeof(ConfigFilter))]
    public IActionResult Index()
    {
        return View();
    }
}
