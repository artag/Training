using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CustomizingRoutingSystem.Infrastucture
{
    public class LegacyRouteForController : IRouter
    {
        private readonly IRouter _mvcRoute;
        private readonly string[] _urls;

        public LegacyRouteForController(IServiceProvider services, params string[] targetUrls)
        {
            // MvcRouteHandler применяется для нахождения контроллеров, действий и возвращения
            // результата клиенту.
            _mvcRoute = services.GetRequiredService<MvcRouteHandler>();
            _urls = targetUrls;
        }

        public async Task RouteAsync(RouteContext context)
        {
            // Получение полного пути URL, с обрезанием косой черты.
            var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');

            // Если запрошенный URL входит в число сконфигурированных URL (_urls)
            if (_urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
            {
                context.RouteData.Values["controller"] = "Legacy";
                context.RouteData.Values["action"] = "GetLegacyUrl";
                context.RouteData.Values["legacyUrl"] = requestedUrl;
                await _mvcRoute.RouteAsync(context);
            }
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (context.Values.ContainsKey("legacyUrl"))
            {
                var url = context.Values["legacyUrl"] as string;
                if (_urls.Contains(url))
                {
                    return new VirtualPathData(this, url);
                }
            }

            return null;
        }
    }
}
