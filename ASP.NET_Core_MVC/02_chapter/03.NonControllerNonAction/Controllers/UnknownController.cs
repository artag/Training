using Microsoft.AspNetCore.Mvc;

namespace NonControllerNonAction.Controllers;

/// <summary>
/// Игнорируемый контроллер.
/// </summary>
[NonController]
public class UnknownController : Controller
{
    public string Name()
    {
        return "Name from controller {nameof(UnknownController)}";
    }

    public ContentResult Index()
    {
        return Content($"This is controller {nameof(UnknownController)}");
    }
}