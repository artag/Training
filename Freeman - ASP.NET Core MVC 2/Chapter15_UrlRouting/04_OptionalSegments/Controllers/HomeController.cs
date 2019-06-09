using Microsoft.AspNetCore.Mvc;
using OptionalSegments.Models;

namespace OptionalSegments.Controllers
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

        public ViewResult CustomVariable(string id)
        {
            var result = new Result
            {
                Controller = nameof(HomeController),
                Action = nameof(CustomVariable)
            };

            result.Data["Id"] = id ?? "<no value>";

            return View("Result", result);
        }
    }
}
