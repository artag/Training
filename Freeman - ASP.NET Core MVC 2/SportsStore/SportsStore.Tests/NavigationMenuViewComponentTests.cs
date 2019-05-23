using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange
            var products = new[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 2, Name = "P2", Category = "Apples" },
                new Product { ProductID = 3, Name = "P3", Category = "Plums" },
                new Product { ProductID = 4, Name = "P4", Category = "Oranges" }
            };

            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());

            var navigationMenu = new NavigationMenuViewComponent(mock.Object);

            // Act
            var viewViewComponentResult = navigationMenu.Invoke() as ViewViewComponentResult;
            var model = viewViewComponentResult.ViewData.Model;
            var results = (model as IEnumerable<string>).ToArray();

            // Assert
            var expected = new[] { "Apples", "Oranges", "Plums" };
            Assert.True(Enumerable.SequenceEqual(expected, results));
        }
    }
}
