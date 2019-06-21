using Microsoft.AspNetCore.Mvc;

namespace PostRedirectGet.Controllers
{
    public class HomeController : Controller
    {
        // Post
        public ViewResult Index() => View("SimpleForm");

        // Redirect
        public RedirectToActionResult ReceiveForm(string name, string city)
        {
            TempData["name"] = name;
            TempData["city"] = city;

            return RedirectToAction(nameof(Data));
        }

        // Get
        public ViewResult Data()
        {
            var name = TempData["name"] as string;
            var city = TempData["city"] as string;

            return View("Result", $"{name} lives in {city}");
        }
    }
}
