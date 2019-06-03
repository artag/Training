using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware.Infrastructure
{
    public class ContentMiddlewareForItems
    {
        private RequestDelegate _nextDelegate;

        public ContentMiddlewareForItems(RequestDelegate next)
        {
            _nextDelegate = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().ToLower() == "/browser")
            {
                var isEdgeBrowser = httpContext.Items["EdgeBrowser"] as bool?;
                if (isEdgeBrowser.HasValue)
                {
                    var message = isEdgeBrowser.Value
                        ? "You using browser Edge"
                        : "You using browser unknown type";

                    await httpContext.Response.WriteAsync(message);
                    return;
                }
            }

            await _nextDelegate.Invoke(httpContext);
        }
    }
}
