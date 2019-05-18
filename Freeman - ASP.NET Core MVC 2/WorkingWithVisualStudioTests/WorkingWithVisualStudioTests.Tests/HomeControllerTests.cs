using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkingWithVisualStudioTests.Controllers;
using WorkingWithVisualStudioTests.Models;
using Xunit;

namespace WorkingWithVisualStudioTests.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexActionModelIsComplete()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var model = GetModel(controller);

            // Assert
            Assert.Equal(SimpleRepository.SharedRepository.Products, model, ProductComparer);
        }

        [Fact]
        public void IndexActionModelIsComplete_UseFakeRepository()
        {
            // Arrange
            var controller = new HomeController(new FakeRepository());

            // Act
            var model = GetModel(controller);

            // Assert
            Assert.Equal(controller.Repository.Products, model, ProductComparer);
        }

        [Fact]
        public void IndexActionModelIsComplete_UseFakeRepositoryPricesUnder50()
        {
            // Arrange
            var controller = new HomeController(new FakeRepositoryPricesUnder50());

            // Act
            var model = GetModel(controller);

            // Assert
            Assert.Equal(controller.Repository.Products, model, ProductComparer);
        }

        [Theory]
        [InlineData(275, 48.95, 19.50, 24.95)]
        [InlineData(5, 48.95, 19.50, 24.95)]
        public void IndexActionModelIsComplete_UseInlineData(
            decimal price1, decimal price2, decimal price3, decimal price4)
        {
            // Arrange
            var products = new Product[]
            {
                new Product { Name = "P1", Price = price1 },
                new Product { Name = "P2", Price = price2 },
                new Product { Name = "P3", Price = price3 },
                new Product { Name = "P4", Price = price4 },
            };

            var repository = new FakeRepository(products);
            var controller = new HomeController(repository);

            // Act
            var model = GetModel(controller);

            // Assert
            Assert.Equal(controller.Repository.Products, model, ProductComparer);
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void IndexActionModelIsComplete_UseProductTestData(Product[] products)
        {
            // Arrange
            var repository = new FakeRepository(products);
            var controller = new HomeController(repository);

            // Act
            var model = GetModel(controller);

            // Assert
            Assert.Equal(controller.Repository.Products, model, ProductComparer);
        }

        [Fact]
        public void RepositoryPropertyCalledOnce_UseFakeRepositoryOnceCall()
        {
            // Arrange
            var repository = new FakeRepositoryPropertyOnceCall();
            var controller = new HomeController(repository);

            // Act
            var result = controller.Index();

            // Assert
            Assert.Equal(expected: 1, actual: repository.PropertyCounter);
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void IndexActionModelIsComplete_UseProductTestData_UseMoq(IEnumerable<Product> products)
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Products).Returns(products);
            var controller = new HomeController(mock.Object);

            // Act
            var model = GetModel(controller);

            // Assert
            Assert.Equal(controller.Repository.Products, model, ProductComparer);
        }

        [Fact]
        public void RepositoryPropertyCalledOnce_UseMoq()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Products)
                .Returns(new [] { new Product { Name = "P1", Price = 100 } });
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            mock.Verify(m => m.Products, Times.Once);
        }

        private IEnumerable<Product> GetModel(HomeController controller) =>
            (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;

        private Comparer<Product> ProductComparer { get; } =
            Comparer.Get<Product>((p1, p2) => p1.Name == p2.Name && p1.Price == p2.Price);
    }
}
