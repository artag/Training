using IntroducingDI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntroducingDI.Controllers
{
    public class InjectionController : Controller
    {
        private readonly IRepository _repository;

        public InjectionController(IRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index() =>
            View("List", _repository.Products);
    }
}
