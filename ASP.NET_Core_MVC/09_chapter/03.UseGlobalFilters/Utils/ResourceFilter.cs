using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Utils;

/// <summary>
/// Фильтр ресурсов. Обычно применяется для переопределения результата действия.
/// </summary>
public class ResourceFilter : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        await OnResourceExecuting(context);
        var response = await next();
        await OnResourceExecuted(response);
    }

    /// <summary>
    /// Срабатывает после фильтров авторизации, но до выполнения метода и
    /// работы фильтров действий, исключений и результатов.
    /// </summary>
    private Task OnResourceExecuting(ResourceExecutingContext context)
    {
        // Получаем информацию о браузере пользователя.
        // Если браузер не IE, передаем обработку запроса дальше.
        var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
        if(Regex.IsMatch(userAgent, "MSIE|Trident"))
        {
            context.Result = new ContentResult { Content = "Ваш браузер устарел" };
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Срабатывает после выполнения метода и фильтров действий, исключений и результатов.
    /// </summary>
    private Task OnResourceExecuted(ResourceExecutedContext context)
    {
        return Task.CompletedTask;
    }
}
