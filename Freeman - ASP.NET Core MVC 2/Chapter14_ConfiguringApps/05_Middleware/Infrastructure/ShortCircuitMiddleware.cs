using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware.Infrastructure
{
    public class ShortCircuitMiddleware
    {
        private RequestDelegate _nextDelegate;

        public ShortCircuitMiddleware(RequestDelegate next)
        {
            _nextDelegate = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers["User-Agent"]
                           .Any(header => header.ToLower().Contains("edge")))
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
