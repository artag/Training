using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Utils;

/// <summary>
/// Фильтры авторизации. Срабатывают после фильтров аутентификации
/// и до запуска остальных фильтров и вызова методов действий.
/// </summary>
public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Выполняется до метода контроллера.
        var text = await context.ReadFromRequest();
        context.SaveToRequest(text, $"{nameof(AuthorizationFilter)}");
    }
}
