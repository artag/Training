using Microsoft.AspNetCore.Mvc;

namespace ReturningDifferentContent.Controllers
{
    public class HomeController : Controller
    {
        public RedirectToActionResult Index() => RedirectToAction("Index", "Json");
    }
}
