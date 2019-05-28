using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        private Mock<IProductRepository> _mock;
        private AdminController _adminController;

        [Fact]
        public void Index_Contains_All_Products()
        {
            // Act
            var result = GetViewModel<IEnumerable<Product>>(Controller.Index())
                ?.ToArray();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            // Act
            var p1 = GetViewModel<Product>(Controller.Edit(1));
            var p2 = GetViewModel<Product>(Controller.Edit(2));
            var p3 = GetViewModel<Product>(Controller.Edit(3));

            // Assert
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3, p3.ProductID);
        }

        [Fact]
        public void Cannot_Edit_NonExistent_Product()
        {
            // Act
            var result = GetViewModel<Product>(Controller.Edit(4));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            // Arrange
            var product = new Product { Name = "Test" };

            // Act
            var result = Controller.Edit(product);

            // Assert
            Mock.Verify(m => m.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange
            var product = new Product { Name = "Test" };

            var controller = new AdminController(Mock.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var result = controller.Edit(product);

            // Assert
            Mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            // Act
            Controller.Delete(2);

            // Assert
            Mock.Verify(m => m.DeleteProduct(2));
        }

        private Product[] Products { get; } =
        {
            new Product { ProductID = 1, Name = "P1" },
            new Product { ProductID = 2, Name = "P2" },
            new Product { ProductID = 3, Name = "P3" },
        };

        private Mock<IProductRepository> Mock
        {
            get
            {
                if (_mock == null)
                {
                    _mock = new Mock<IProductRepository>();
                    _mock.Setup(m => m.Products).Returns(Products.AsQueryable());
                }

                return _mock;
            }
        }

        private AdminController Controller
        {
            get
            {
                if (_adminController == null)
                {
                    var mockTempData = new Mock<ITempDataDictionary>();

                    _adminController = new AdminController(Mock.Object);
                    _adminController.TempData = mockTempData.Object;
                }

                return _adminController;
            }
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            var viewResult = result as ViewResult;
            return viewResult?.ViewData.Model as T;
        }
    }
}
