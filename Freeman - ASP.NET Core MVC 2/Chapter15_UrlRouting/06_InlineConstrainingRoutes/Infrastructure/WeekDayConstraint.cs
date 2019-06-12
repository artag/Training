using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InlineConstrainingRoutes.Infrastructure
{
    public class WeekDayConstraint : IRouteConstraint
    {
        private static string[] Days = new[]
        {
            "mon", "tue", "wed", "thu", "fri", "sat", "sun"
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="httpContext">Запрос, поступивший от клиента.</param>
        /// <param name="route">Маршрут.</param>
        /// <param name="routeKey">Имя ограничиваемого сегмента.</param>
        /// <param name="values">Переменные сегментов, извлеченные из URL.</param>
        /// <param name="routeDirection">Тип URL: входящий или исходящий.</param>
        /// <returns></returns>
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return Days.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }
    }
}
