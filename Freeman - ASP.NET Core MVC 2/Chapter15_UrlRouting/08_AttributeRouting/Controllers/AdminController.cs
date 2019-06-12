using AttributeRouting.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttributeRouting.Controllers
{
    [Route("app/[controller]/actions/[action]/{id?}")]
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

        public ViewResult List(string id)
        {
            var result = new Result
            {
                Controller = nameof(AdminController),
                Action = nameof(List)
            };

            result.Data["Id"] = id ?? "<no value>";

            return View("Result", result);
        }
    }
}
