using Microsoft.AspNetCore.Mvc;

namespace PerformingRedirections.Controllers
{
    public class OtherController : Controller
    {
        public ViewResult ActionRedirection() =>
            View("Result", $"Result from {nameof(OtherController)}.{nameof(ActionRedirection)}");
    }
}