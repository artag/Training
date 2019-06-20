using Microsoft.AspNetCore.Mvc;

namespace PerformingRedirections.Controllers
{
    public class RedirectionController : Controller
    {
        public RedirectResult Redirect() => Redirect("/Redirection/LiteralUrl");

        public ViewResult LiteralUrl() => View("Result", $"Result from {nameof(LiteralUrl)}");

        public RedirectToRouteResult RedirectToRoute()
        {
            var route = new
            {
                controller = "Redirection", action = "RoutedRedirection", id = "MyID"
            };

            return RedirectToRoute(route);
        }

        public ViewResult RoutedRedirection() =>
            View("Result", $"Result from {nameof(RoutedRedirection)}");

        public RedirectToActionResult RedirectToAction() =>
            RedirectToAction(nameof(ActionRedirection));

        public ViewResult ActionRedirection() =>
            View("Result", $"Result from {nameof(ActionRedirection)}");
    }
}
