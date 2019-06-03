using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware.Infrastructure
{
    public class ContentMiddlewareWithService
    {
        private readonly RequestDelegate _nextDelegate;
        private readonly UptimeService _uptimeService;

        public ContentMiddlewareWithService(RequestDelegate next, UptimeService uptimeService)
        {
            _nextDelegate = next;
            _uptimeService = uptimeService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().ToLower() == "/timer")
            {
                var message = "This is from the content middleware " +
                              $"(uptime: {_uptimeService.Uptime}ms)";

                await httpContext.Response.WriteAsync(message, Encoding.UTF8);
            }
            else
            {
                await _nextDelegate.Invoke(httpContext);
            }
        }
    }
}
