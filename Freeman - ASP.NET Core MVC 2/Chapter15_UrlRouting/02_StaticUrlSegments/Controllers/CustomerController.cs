using Microsoft.AspNetCore.Mvc;
using StaticUrlSegments.Models;

namespace StaticUrlSegments.Controllers
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

        public ViewResult List()
        {
            var result = new Result
            {
                Controller = nameof(CustomerController),
                Action = nameof(List)
            };

            return View("Result", result);
        }
    }
}
