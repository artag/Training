using Microsoft.AspNetCore.Mvc;

namespace PerformingRedirections.Controllers
{
    public class RedirectionController : Controller
    {
        // ������ ������ Redirect()
        public RedirectResult ActionRedirect() =>
            Redirect("/Redirection/LiteralUrl");

        public ViewResult LiteralUrl() =>
            View("Result", $"Result from {nameof(LiteralUrl)}");


        // ������ ������ RedirectToRoute()
        public RedirectToRouteResult ActionRedirectToRoute()
        {
            var route = new
            {
                controller = "Redirection", action = "RoutedRedirection", id = "MyID"
            };

            return RedirectToRoute(route);
        }

        public ViewResult RoutedRedirection() =>
            View("Result", $"Result from {nameof(RoutedRedirection)}");


        // ������ ������ RedirectToAction()
        public RedirectToActionResult ActionRedirectToAction() =>
            RedirectToAction(nameof(ActionRedirection));

        public ViewResult ActionRedirection() =>
            View("Result", $"Result from {nameof(ActionRedirection)}");
    }
}
