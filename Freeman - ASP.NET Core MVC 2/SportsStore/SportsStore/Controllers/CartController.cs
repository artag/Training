using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _repository;
        private Cart _cartService;

        public CartController(IProductRepository repository, Cart cartService)
        {
            _repository = repository;
            _cartService = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = _cartService,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            var product = _repository.Products
                                     .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                _cartService.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = _repository.Products
                                     .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                _cartService.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
