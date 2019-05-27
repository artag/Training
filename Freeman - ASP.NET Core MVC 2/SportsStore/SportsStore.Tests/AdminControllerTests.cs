using Moq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            // Arrange
            var products = new[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
            };

            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products).AsQueryable<Product>();

            // Act

            // Assert
        }
    }
}
