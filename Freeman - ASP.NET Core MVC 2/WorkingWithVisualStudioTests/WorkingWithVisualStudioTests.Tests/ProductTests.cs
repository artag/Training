using WorkingWithVisualStudioTests.Models;
using Xunit;

namespace WorkingWithVisualStudioTests.Tests
{
    public class ProductTests
    {
        [Fact]
        public void CanChangeProductName()
        {
            // Arrange
            var product = new Product { Name = "Test", Price = 100M };

            // Act
            product.Name = "New Name";

            // Assert
            Assert.Equal(expected: "New Name", actual: product.Name);
        }

        [Fact]
        public void CanChangeProductPrice()
        {
            // Arrange
            var product = new Product { Name = "Test", Price = 100M };

            // Act
            product.Price = 200M;

            // Assert
            Assert.Equal(expected: 200M, actual: product.Price);
        }
    }
}
