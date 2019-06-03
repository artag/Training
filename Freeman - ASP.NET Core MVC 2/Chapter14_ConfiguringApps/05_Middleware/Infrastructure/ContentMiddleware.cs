using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware.Infrastructure
{
    public class ContentMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public ContentMiddleware(RequestDelegate next)
        {
            _nextDelegate = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().ToLower() == "/content")
            {
                var message = "This is from the content middleware";
                await httpContext.Response.WriteAsync(message, Encoding.UTF8);
            }
            else
            {
                await _nextDelegate.Invoke(httpContext);
            }
        }
    }
}
