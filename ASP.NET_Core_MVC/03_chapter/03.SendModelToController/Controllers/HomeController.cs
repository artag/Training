using Microsoft.AspNetCore.Mvc;

namespace SendModelToController.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var message =
            "Доступные адреса:\n" +
            "https://localhost:5001/documents/use-properties - Передача данных в контроллер в виде набора полей\n" +
            "https://localhost:5001/documents/use-class      - Передача данных в контроллер в виде сложного объекта (класса)\n" +
            "https://localhost:5001/documents/use-classes    - Передача данных в контроллер в виде сложных объектов (массива классов)";

        return Content(message);
    }
}
