using Microsoft.AspNetCore.Mvc;

namespace ReceivingContextData.Controllers
{
    public class InputController : Controller
    {
        public ViewResult IndexFirst() => View("SimpleFormFirst");

        public ViewResult IndexSecond() => View("SimpleFormSecond");

        public ViewResult ReceiveFormFirst()
        {
            var name = Request.Form["name"];
            var city = Request.Form["city"];
            return View("Result", $"{name} lives in {city}");
        }

        public ViewResult ReceiveFormSecond(string name, string city) =>
            View("Result", $"{name} lives in {city}");
    }
}
