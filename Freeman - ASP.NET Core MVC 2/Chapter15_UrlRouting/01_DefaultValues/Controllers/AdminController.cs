using DefaultValues.Models;
using Microsoft.AspNetCore.Mvc;

namespace DefaultValues.Controllers
{
    public class AdminController : Controller
    {
        public ViewResult Index()
        {
            var result = new Result
            {
                Controller = nameof(AdminController),
                Action = nameof(Index)
            };

            return View("Result", result);
        }
    }
}
