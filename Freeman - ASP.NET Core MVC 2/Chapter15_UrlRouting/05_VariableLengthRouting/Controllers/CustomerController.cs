using Microsoft.AspNetCore.Mvc;
using VariableLengthRouting.Models;

namespace VariableLengthRouting.Controllers
{
    public class CustomerController : Controller
    {
        public ViewResult Index()
        {
            var result = new Result
            {
                Controller = nameof(CustomerController),
                Action = nameof(Index)
            };

            return View("Result", result);
        }

        public ViewResult List(string id)
        {
            var result = new Result
            {
                Controller = nameof(CustomerController),
                Action = nameof(List)
            };

            result.Data["Id"] = id ?? "<no value>";
            result.Data["catchall"] = RouteData.Values["catchall"];

            return View("Result", result);
        }
    }
}
