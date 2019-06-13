using GenerateOutgoingLink.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenerateOutgoingLink.Controllers
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

        public ViewResult GenerateUrl(int id)
        {
            var result = new Result
            {
                Controller = nameof(AdminController),
                Action = nameof(Index)
            };

            result.Data["Id"] = id;
            result.Data["Url"] = Url.Action("GenerateUrl", "Admin", new { id = 100 });

            return View("Result", result);
        }
    }
}
