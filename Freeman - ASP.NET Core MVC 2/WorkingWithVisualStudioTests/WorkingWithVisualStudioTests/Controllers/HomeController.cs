using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WorkingWithVisualStudioTests.Models;

namespace WorkingWithVisualStudioTests.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() : this(SimpleRepository.SharedRepository)
        {
        }

        public HomeController(IRepository repository)
        {
            Repository = repository;
        }

        public IRepository Repository { get; }

        public IActionResult Index() => View(Repository.Products);

        [HttpGet]
        public IActionResult AddProduct() => View(new Product());

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            Repository.AddProduct(product);
            return RedirectToAction("Index");
        }
    }
}
