using Microsoft.AspNetCore.Mvc;
using UsingActionInjection.Models;

namespace UsingActionInjection.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index([FromServices]ProductTotalizer totalizer)
        {
            ViewBag.Total = totalizer.Total;

            return View(_repository.Products);
        }
    }
}
