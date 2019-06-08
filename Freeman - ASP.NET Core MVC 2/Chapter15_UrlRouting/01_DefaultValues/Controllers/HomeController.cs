using DefaultValues.Models;
using Microsoft.AspNetCore.Mvc;

namespace DefaultValues.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            var result = new Result
            {
                Controller = nameof(HomeController),
                Action = nameof(Index),
            };

            return View("Result", result);
        }
    }
}
