using Microsoft.AspNetCore.Mvc;

namespace DIForConcreteType.Controllers
{
    public class HomeController : Controller
    {
        public RedirectToActionResult Index() =>
            RedirectToAction(nameof(InjectionController.Index),
                             nameof(InjectionController).Replace("Controller", ""));
    }
}
