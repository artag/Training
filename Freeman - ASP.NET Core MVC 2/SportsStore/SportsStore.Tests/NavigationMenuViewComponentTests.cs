using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        private Mock<IProductRepository> _mock;

        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange
            var navigationMenu = new NavigationMenuViewComponent(Mock.Object);

            // Act
            var viewViewComponentResult = navigationMenu.Invoke() as ViewViewComponentResult;
            var model = viewViewComponentResult.ViewData.Model;
            var results = (model as IEnumerable<string>).ToArray();

            // Assert
            var expected = new[] { "Apples", "Oranges", "Plums" };
            Assert.True(Enumerable.SequenceEqual(expected, results));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            // Arrange
            var categoryToSelect = "Apples";

            var navigationMenu = new NavigationMenuViewComponent(Mock.Object);
            navigationMenu.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext { RouteData = new RouteData()}
            };

            navigationMenu.RouteData.Values["category"] = categoryToSelect;

            // Act
            var viewViewComponentResult = navigationMenu.Invoke() as ViewViewComponentResult;
            var result = viewViewComponentResult.ViewData["SelectedCategory"] as string;

            // Assert
            Assert.Equal(categoryToSelect, result);
        }

        private IEnumerable<Product> Products { get; } = new[]
        {
            new Product { ProductID = 1, Name = "P1", Category = "Apples" },
            new Product { ProductID = 2, Name = "P2", Category = "Apples" },
            new Product { ProductID = 3, Name = "P3", Category = "Plums" },
            new Product { ProductID = 4, Name = "P4", Category = "Oranges" }
        };

        private Mock<IProductRepository> Mock
        {
            get
            {
                if (_mock == null)
                {
                    _mock = new Mock<IProductRepository>();
                    _mock.Setup(m => m.Products).Returns(Products.AsQueryable<Product>());
                }

                return _mock;
            }
        }
    }
}
