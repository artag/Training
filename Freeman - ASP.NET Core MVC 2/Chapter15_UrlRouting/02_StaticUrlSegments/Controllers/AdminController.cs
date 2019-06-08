using Microsoft.AspNetCore.Mvc;
using StaticUrlSegments.Models;

namespace StaticUrlSegments.Controllers
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
