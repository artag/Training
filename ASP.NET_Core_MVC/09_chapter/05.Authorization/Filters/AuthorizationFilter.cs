using Authorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authorization.Filters;

/// <summary>
/// Фильтр действия (авторизации).
/// </summary>
public class AuthorizationFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authExists = IsAuthenticationExists(context);

        if (!authExists)
        {
            // Пользователь неавторизован, переход к представлению ввода данных для аутентификации.
            var url = new RouteValueDictionary(new { controller = "Home", action = "Login" });
            context.Result = new RedirectToRouteResult(url);
        }
        else
        {
            // Пользователь авторизован.
            await next();
        }
    }

    private static bool IsAuthenticationExists(ActionExecutingContext context)
    {
        // Попытка считать переменную сессии с информацией об авторизации пользователя.
        if (context.HttpContext.Session.GetString(Constants.UserId) != null)
            return true;

        // В сессии и в cookie нет информации - пользователь неавторизован.
        if (!context.HttpContext.Request.Cookies.ContainsKey(Constants.UserId))
            return false;

        // В cookie есть информация. Обновление информации в сессии.
        var cookie = context.HttpContext.Request.Cookies[Constants.UserId] ?? string.Empty;
        context.HttpContext.Session.SetString(Constants.UserId, cookie);
        return true;
    }
}
