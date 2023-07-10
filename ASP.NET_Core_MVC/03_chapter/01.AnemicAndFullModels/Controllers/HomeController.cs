using Microsoft.AspNetCore.Mvc;

namespace AnemicAndFullModels.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var message =
            "Адреса:\n" +
            "https://localhost:5001/model/anemic    - Пример анемичной (тонкой) модели (класс Auto)\n" +
            "https://localhost:5001/model/full      - Пример полной (богатой) модели (класс ThickAuto)";

        return Content(message);
    }
}
