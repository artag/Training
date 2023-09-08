using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FiltersSample.Filters;

/// <summary>
/// Фильтр действия.<br/>
/// Проверяет и блокирует запрос из браузеров старых типов.
/// </summary>
/// <remarks>
/// Наследует <see cref="Attribute"/>, т.к. фильтры удобно применять как атрибуты.
/// </remarks>
public class BrowserFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

        // Проверка агента для IE и старого Edge.
        if (Regex.IsMatch(userAgent, "MSIE|Trident|Edge"))
        {
            // Установка результата и прерывание запроса.
            context.Result = new ObjectResult("Ваш браузер устарел!");
        }
        else
        {
            // Передача управления следующему фильтру при отсутствии других фильтров.
            await next();
        }
    }
}
