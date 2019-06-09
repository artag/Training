using CustomSegmentVariables.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomSegmentVariables.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            var result = new Result
            {
                Controller = nameof(HomeController),
                Action = nameof(Index)
            };

            return View("Result", result);
        }

        public ViewResult CustomVariable()
        {
            var result = new Result
            {
                Controller = nameof(HomeController),
                Action = nameof(CustomVariable)
            };

            result.Data["Id"] = RouteData.Values["id"];

            return View("Result", result);
        }

        public ViewResult CustomVariable2(string id)
        {
            var result = new Result
            {
                Controller = nameof(HomeController),
                Action = nameof(CustomVariable2)
            };

            result.Data["Id"] = id;

            return View("Result", result);
        }
    }
}
