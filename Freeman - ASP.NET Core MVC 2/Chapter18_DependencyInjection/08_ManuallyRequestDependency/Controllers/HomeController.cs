using Microsoft.AspNetCore.Mvc;
using ManuallyRequestDependency.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ManuallyRequestDependency.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var repository = HttpContext.RequestServices.GetService<IRepository>();
            return View(repository.Products);
        }
    }
}
