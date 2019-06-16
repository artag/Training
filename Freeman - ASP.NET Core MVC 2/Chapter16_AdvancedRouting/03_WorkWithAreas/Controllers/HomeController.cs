using Microsoft.AspNetCore.Mvc;
using WorkWithAreas.Models;

namespace WorkWithAreas.Controllers
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
