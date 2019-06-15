using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CustomizingRoutingSystem.Infrastucture
{
    public class LegacyRoute : IRouter
    {
        private readonly string[] _urls;

        public LegacyRoute(params string[] targetUrls)
        {
            _urls = targetUrls;
        }

        public Task RouteAsync(RouteContext context)
        {
            // Получение полного пути URL, с обрезанием косой черты.
            var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');

            // Если запрошенный URL входит в число сконфигурированных URL (_urls)
            if (_urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
            {
                // Установка свойства Handler с применением лямбда-функции, генерирующей ответ.
                context.Handler = async ctx =>
                {
                    HttpResponse response = ctx.Response;
                    byte[] bytes = Encoding.ASCII.GetBytes($"URL: {requestedUrl}");
                    await response.Body.WriteAsync(bytes, 0, bytes.Length);
                };
            }

            return Task.CompletedTask;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }
    }
}
