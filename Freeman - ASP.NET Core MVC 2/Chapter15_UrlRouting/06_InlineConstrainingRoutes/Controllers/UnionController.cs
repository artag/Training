using InlineConstrainingRoutes.Models;
using Microsoft.AspNetCore.Mvc;

namespace InlineConstrainingRoutes.Controllers
{
    public class UnionController : Controller
    {
        public ViewResult Index(string id)
        {
            var result = new Result
            {
                Controller = nameof(UnionController),
                Action = nameof(Index)
            };

            result.Data["Id"] = id;

            return View("Result", result);
        }
    }
}
