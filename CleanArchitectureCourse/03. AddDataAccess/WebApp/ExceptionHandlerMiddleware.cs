using System.Net;
using Application;

namespace WebApp;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (EntityNotFoundException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ExceptionHandlerMiddleware>();
}