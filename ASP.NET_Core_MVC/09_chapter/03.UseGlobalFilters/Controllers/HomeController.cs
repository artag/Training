using Microsoft.AspNetCore.Mvc;

namespace Filters.Controllers;

public class HomeController : Controller
{
    public IActionResult Index(int id)
    {
        var body = HttpContext.Request.Body;

        using var reader = new StreamReader(body);
        var text = reader.ReadToEnd();
        var response = $"{text}, {nameof(Index)}";

        // Проверка работы ExceptionFilter.
        if (id > 0)
            throw new InvalidOperationException("Some internal error");

        return Content(response);
    }
}
