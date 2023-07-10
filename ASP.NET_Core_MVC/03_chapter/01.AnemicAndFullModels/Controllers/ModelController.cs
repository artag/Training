using AnemicAndFullModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnemicAndFullModels.Controllers;

public class ModelController : Controller
{
    public IActionResult Anemic()
    {
        var auto = new Auto
        {
            Brand = "Audi",
            ModelAuto = "A6",
            EngineCapacity = 2393,
        };

        var result = $"Анемичная (тонкая) модель {nameof(Auto)}:\n" +
                     $"Наименование: {auto.Brand}," +
                     $"Модель: {auto.ModelAuto}," +
                     $"Объем двигателя, куб. см: {auto.EngineCapacity}";

        return Content(result);
    }

    public IActionResult Full()
    {
        var auto = new ThickAuto("Audi", "A6", 2393);
        var result = auto.GetInfo();
        return Content(result);
    }
}
