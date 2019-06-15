using CustomizingRoutingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomizingRoutingSystem.Controllers
{
    public class HomeController : Controller
    {
        private const string Name = "Home";

        public ViewResult Index()
        {
            var result = new Result
            {
                Controller = Name,
                Action = nameof(Index)
            };

            return View("Result", result);
        }

        public ViewResult List(string id)
        {
            var result = new Result
            {
                Controller = Name,
                Action = nameof(List)
            };

            result.Data["Id"] = id;

            return View("Result", result);
        }
    }
}
