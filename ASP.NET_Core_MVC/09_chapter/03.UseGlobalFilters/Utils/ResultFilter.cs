using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Utils;

/// <summary>
///  Фильтры результатов выполняются только тогда, когда выполнение метода завершилось успешно.
/// </summary>
public class ResultFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await OnResultExecuting(context);
        var response = await next();
        await OnResultExecuted(response);
    }

    /// <summary>
    /// Вызывается перед выполнением результата метода.
    /// Можно предотвратить дальнейшее выполнение фильтров и обработку запроса,
    /// установив свойство ResultExecutingContext.Cancel равным true.
    /// </summary>
    private Task OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not ContentResult result)
            return Task.CompletedTask;

        var modified = string.Format("{0}, {1}_after", result.Content, nameof(ResultFilter));
        context.Result = new ContentResult { Content = modified };
        return Task.CompletedTask;
    }

    /// <summary>
    /// Вызывается после выполнения результата метода.
    /// Также можно предотвратить дальнейшую обработку запроса на других фильтрах,
    /// присвоив свойству ResultExecutedContext.Canceled значение true.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private Task OnResultExecuted(ResultExecutedContext context)
    {
        return Task.CompletedTask;
    }
}
