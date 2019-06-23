using Microsoft.AspNetCore.Mvc;

namespace IntroducingDI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() =>
            RedirectToAction(nameof(InjectionController.Index),
                             nameof(InjectionController).Replace("Controller", ""));
    }
}
