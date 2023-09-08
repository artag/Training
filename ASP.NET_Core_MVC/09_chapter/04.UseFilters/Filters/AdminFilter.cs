using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FiltersSample.Filters;

/// <summary>
/// Фильтр действия.<br/>
/// Пример получения значений извне.
/// </summary>
/// <remarks>
/// Наследует <see cref="Attribute"/>, т.к. фильтры удобно применять как атрибуты.
/// </remarks>
public class AdminFilter : Attribute, IAsyncActionFilter
{
    private readonly string _user;

    public AdminFilter(string user)
    {
        _user = user;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (_user.ToLower().Contains("admin"))
        {
            // Прерывание запроса.
            context.Result = new ObjectResult("Привет, Админ!");
        }
        else
        {
            // Передача управления далее.
            await next();
        }
    }
}
