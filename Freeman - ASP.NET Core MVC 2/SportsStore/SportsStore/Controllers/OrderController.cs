using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Cart _cartService;

        public OrderController(IOrderRepository orderRepository, Cart cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
        }

        [Authorize]
        public ViewResult List() => View(_orderRepository.Orders.Where(order => !order.Shipped));

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID)
        {
            var order = _orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderID);
            if (order != null)
            {
                order.Shipped = true;
                _orderRepository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));
        }

        public IActionResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!_cartService.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = _cartService.Lines.ToArray();
                _orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            _cartService.Clear();
            return View();
        }
    }
}
