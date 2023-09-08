using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Utils;

/// <summary>
/// Фильтры действий выполняются после фильтров авторизации и ресурсов и уже после привязки модели.
/// Данные фильтры могут использоваться для работы с привязкой модели,
/// модификации входных данных в метод контроллера или его результата.
/// </summary>
public class ActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await OnActionExecuting(context);
        var response = await next();
        await OnActionExecuted(response);
    }

    /// <summary>
    /// Вызывается до выполнения метода контроллера, после фильтров авторизации и ресурсов.
    /// Можно манипулировать параметрами метода, управлять контроллером или завершить обработку запроса.
    /// (См. свойства ActionArguments, Controller, Result).
    /// </summary>
    private async Task OnActionExecuting(ActionExecutingContext context)
    {
        var text = await context.ReadFromRequest();
        context.SaveToRequest(text, $"{nameof(ActionFilter)}_before");
    }

    /// <summary>
    /// Вызывается после выполнения метода.
    /// Здесь можно посмотреть или изменить результат его выполнения.
    /// (См. свойство Result).
    /// </summary>
    private Task OnActionExecuted(ActionExecutedContext response)
    {
        if (response.Result is not ContentResult result)
            return Task.CompletedTask;

        var modified = string.Format("{0}, {1}_after", result.Content, nameof(ActionFilter));
        response.Result = new ContentResult { Content = modified };
        return Task.CompletedTask;
    }
}
