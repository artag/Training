using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository) => _repository = repository;

        public int PageSize { get; set; } = 4;

        public ViewResult List(int productPage = 1) =>
            View(_repository.Products
                .OrderBy(product => product.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize));
    }
}