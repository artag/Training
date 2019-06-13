using GenerateOutgoingLink.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenerateOutgoingLink.Controllers
{
    [Route("app/[controller]/actions/[action]/{id?}")]
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
    }
}
