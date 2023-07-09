using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace HttpContextFromController.Controllers;

[Route("")]
[Route("context")]
public class ContextController : Controller
{
    [Route("headers")]
    public ContentResult GetAllHeaders()
    {
        var headers = HttpContext.Request.Headers;
        var content = new StringBuilder();
        foreach (var header in headers)
        {
            content.AppendLine($"{header.Key}={header.Value}");
        }

        return Content(content.ToString());
    }

    [Route("method")]
    public ContentResult GetMethod()
    {
        var content = Request.Method;        // Равнозначен HttpContext.Request.Method
        return Content(content);
    }

    [Route("route")]
    public ContentResult GetRouteData()
    {
        var content = GetRouteDataInternal();
        return Content(content.ToString());
    }

    [Route("params/{id?}/{*catchall}")]
    public ContentResult GetManyParams()
    {
        var content = GetRouteDataInternal();
        return Content(content.ToString());
    }

    [Route("")]
    [Route("index")]
    public ContentResult Index()
    {
        var message =
            "Addresses:\n" +
            "https://localhost:5001/context/headers                  - Вывести все заголовки запроса\n" +
            "https://localhost:5001/context/method                   - Каким методом передан запрос на обработку\n" +
            "https://localhost:5001/context/route                    - Вывести параметры маршрута\n" +
            "https://localhost:5001/context/params/{id?}/par1/par2   - Получить любое множество параметров из запроса";

        return Content(message);
    }

    private StringBuilder GetRouteDataInternal()
    {
        var content = new StringBuilder();
        var data = HttpContext.GetRouteData();
        foreach (var item in data.Values)
        {
            content.AppendLine($"{item.Key}={item.Value}");
        }

        return content;
    }
}
