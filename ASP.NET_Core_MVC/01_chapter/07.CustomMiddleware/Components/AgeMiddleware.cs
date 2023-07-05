using Microsoft.Extensions.Primitives;

namespace AgeMiddleware.Components;

/// <summary>
/// Пользовательский компонент Middleware.
/// </summary>
public class AgeMiddleware
{
    private readonly RequestDelegate _next;

    public AgeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var query = context.Request.Query["age"];
        var age = GetAge(query);
        if (age < 18)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Age is invalid");
        }
        else
        {
            await _next.Invoke(context);
        }
    }

    private int GetAge(StringValues query) =>
        string.IsNullOrEmpty(query)
            ? 0
            : TryToConvert(query);

    private int TryToConvert(StringValues query)
    {
        try
        {
            var value = Convert.ToInt32(query);
            return value;
        }
        catch (FormatException)
        {
            return 0;
        }
        catch (OverflowException)
        {
            return 0;
        }
    }
}
