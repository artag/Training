using Microsoft.AspNetCore.Mvc;

namespace CustomizingRoutingSystem.Controllers
{
    public class LegacyController : Controller
    {
        // legacyUrl - Унаследованный URL, запрошенный клиентом</param>
        public ViewResult GetLegacyUrl(string legacyUrl) =>
            View((object)legacyUrl);
    }
}
