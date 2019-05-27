using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            var order = new Order();
            var controller = new OrderController(mock.Object, cart);

            // Act
            var result = controller.Checkout(order) as ViewResult;

            // Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();

            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            var controller = new OrderController(mock.Object, cart);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var result = controller.Checkout(new Order()) as ViewResult;

            // Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();

            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            var controller = new OrderController(mock.Object, cart);

            // Act
            var result = controller.Checkout(new Order()) as RedirectToActionResult;

            // Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
