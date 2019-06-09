using Microsoft.AspNetCore.Mvc;
using VariableLengthRouting.Models;

namespace VariableLengthRouting.Controllers
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
    }
}
