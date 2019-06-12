using InlineConstrainingRoutes.Models;
using Microsoft.AspNetCore.Mvc;

namespace InlineConstrainingRoutes.Controllers
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

        public ViewResult CustomVariable(int id)
        {
            var result = new Result
            {
                Controller = nameof(HomeController),
                Action = nameof(CustomVariable)
            };

            result.Data["Id"] = id;

            return View("Result", result);
        }
    }
}
