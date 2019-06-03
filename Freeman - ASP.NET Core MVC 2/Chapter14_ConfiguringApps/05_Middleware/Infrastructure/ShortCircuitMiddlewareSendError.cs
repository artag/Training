using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware.Infrastructure
{
    public class ShortCircuitMiddlewareSendError
    {
        private readonly RequestDelegate _nextDelegate;

        public ShortCircuitMiddlewareSendError(RequestDelegate next)
        {
            _nextDelegate = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().ToLower() == "/error")
            {
                httpContext.Response.StatusCode = 403;
            }
            else
            {
                await _nextDelegate.Invoke(httpContext);
            }
        }
    }
}
