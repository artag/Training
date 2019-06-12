using InlineConstrainingRoutes.Models;
using Microsoft.AspNetCore.Mvc;

namespace InlineConstrainingRoutes.Controllers
{
    public class AdminController : Controller
    {
        public ViewResult CustomVariable(int id)
        {
            var result = new Result
            {
                Controller = nameof(AdminController),
                Action = nameof(CustomVariable)
            };

            result.Data["Id"] = id;

            var idInRange = 10 <= id && id <= 20;
            result.Data["InRange"] = idInRange;

            return View("Result", result);
        }
    }
}
