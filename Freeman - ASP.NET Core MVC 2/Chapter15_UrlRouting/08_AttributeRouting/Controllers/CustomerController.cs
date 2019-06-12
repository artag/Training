using AttributeRouting.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttributeRouting.Controllers
{
    public class CustomerController : Controller
    {
        [Route("myroute")]
        public ViewResult Index()
        {
            var result = new Result
            {
                Controller = nameof(CustomerController),
                Action = nameof(Index)
            };

            return View("Result", result);
        }

        [Route("[controller]/MyAction")]
        public ViewResult Index2()
        {
            var result = new Result
            {
                Controller = nameof(CustomerController),
                Action = nameof(Index2)
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

            return View("Result", result);
        }
    }
}
