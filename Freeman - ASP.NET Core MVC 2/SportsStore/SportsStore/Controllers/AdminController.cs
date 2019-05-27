using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;

        public AdminController(IProductRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index() => View(_repository.Products);

        public ViewResult Edit(int productId) =>
            View(_repository.Products
                            .FirstOrDefault(product => product.ProductID == productId));
    }
}
