using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware.Infrastructure
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public ErrorMiddleware(RequestDelegate next)
        {
            _nextDelegate = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _nextDelegate.Invoke(httpContext);

            if (httpContext.Response.StatusCode == 403)
            {
                await httpContext.Response.WriteAsync("The code 403 was sended", Encoding.UTF8);
            }
            else if (httpContext.Response.StatusCode == 404)
            {
                var message = "The code 404 was sended. No content middleware response";
                await httpContext.Response.WriteAsync(message, Encoding.UTF8);
            }
        }
    }
}
