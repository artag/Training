using InlineConstrainingRoutes.Models;
using Microsoft.AspNetCore.Mvc;

namespace InlineConstrainingRoutes.Controllers
{
    public class WeekDayController : Controller
    {
        public ViewResult Index(string id)
        {
            var result = new Result
            {
                Controller = nameof(WeekDayController),
                Action = nameof(Index)
            };

            result.Data["Id"] = id ?? "empty";

            return View("Result", result);
        }
    }
}
