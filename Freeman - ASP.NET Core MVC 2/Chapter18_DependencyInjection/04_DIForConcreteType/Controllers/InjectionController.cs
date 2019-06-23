using DIForConcreteType.Models;
using Microsoft.AspNetCore.Mvc;

namespace DIForConcreteType.Controllers
{
    public class InjectionController : Controller
    {
        private readonly IRepository _repository;
        private readonly ProductTotalizer _totalizer;

        public InjectionController(IRepository repository, ProductTotalizer totalizer)
        {
            _repository = repository;
            _totalizer = totalizer;
        }

        public ViewResult Index()
        {
            ViewBag.Total = _totalizer.Total;
            return View("List", _repository.Products);
        }
    }
}
