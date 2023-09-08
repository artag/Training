using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Utils;

/// <summary>
/// Фильтры исключений обрабатывают необработанные исключения, в том числе те,
/// которые возникли при создании контроллера и привязки модели.
/// Обрабатывают только исключения, которые возникают при вызове метода контроллера в MVC.
/// </summary>
public class ExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        // Обработка исключений, если метод контроллера бросил исключение.
        var message = context.Exception.Message;
        var modified = string.Format("From {0} exception message:\n{1}", nameof(ExceptionFilter), message);
        context.Result = new ContentResult { Content = modified };
        return Task.CompletedTask;
    }
}
