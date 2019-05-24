using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository) => _repository = repository;

        public int PageSize { get; set; } = 4;

        public ViewResult List(string category, int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products = _repository.Products
                    .Where(product => category == null || product.Category == category)
                    .OrderBy(product => product.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null
                        ? _repository.Products.Count()
                        : _repository.Products
                            .Count(product => product.Category == category)
                },
                CurrentCategory = category
            });
    }
}
