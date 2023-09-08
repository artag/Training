using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FiltersSample.Filters;

/// <summary>
/// Фильтр действия.<br/>
/// Пример получения зависимости через механизм DI.<br/>
/// Приводится пример чтения и использование параметра из конфигурации для фильтрации.
/// </summary>
/// <remarks>
/// Наследует <see cref="Attribute"/>, т.к. фильтры удобно применять как атрибуты.
/// </remarks>
public class ConfigFilter : Attribute, IAsyncActionFilter
{
    private readonly IConfiguration _config;

    public ConfigFilter(IConfiguration config)
    {
        _config = config;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Чтение параметра из файла конфигурации.
        var prm = _config["CodeCountry"]?.ToLower() ?? string.Empty;
        if (prm != "rus")
        {
            // Прерывание запроса.
            context.Result = new ObjectResult("Доступ запрещен!");
        }
        else
        {
            // Передача управления далее.
            await next();
        }
    }
}
