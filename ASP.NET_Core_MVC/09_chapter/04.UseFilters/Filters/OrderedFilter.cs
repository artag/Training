using Microsoft.AspNetCore.Mvc.Filters;

namespace FiltersSample.Filters;

/// <summary>
/// При установке Order = 1 выполнится после <see cref="BrowserFilter"/>.
/// </summary>
/// <remarks>
/// Наследует <see cref="Attribute"/>, т.к. фильтры удобно применять как атрибуты.<br/>
/// Все фильтры по умолчанию имеют Order = 0 - наивысший приоритет.
/// </remarks>
public class OrderedFilter : Attribute, IActionFilter, IOrderedFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public int Order { get; set; }
}
