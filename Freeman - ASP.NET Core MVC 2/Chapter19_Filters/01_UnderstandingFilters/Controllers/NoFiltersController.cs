using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnderstandingFilters.Controllers
{
    public class NoFiltersController : Controller
    {
        // А так будет, если не использовать фильтр, а действовать напрямую, через контроллер
        public IActionResult Index()
        {
            if (!Request.IsHttps)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            return View("Message", "This is the Index action on the NoFilters controller");
        }
    }
}
