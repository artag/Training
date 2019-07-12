using CreatingViewComponent.Models;
using Microsoft.AspNetCore.Mvc;

namespace CreatingViewComponent.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _repository;

        public HomeController(IProductRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index() => View(_repository.Products);

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product newProduct)
        {
            _repository.AddProduct(newProduct);
            return RedirectToAction(nameof(Index));
        }
    }
}
