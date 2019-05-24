using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTest
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(Product1, 1);
            cart.AddItem(Product2, 1);

            var results = cart.Lines.ToArray();

            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(Product1, results[0].Product);
            Assert.Equal(Product2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(Product1, 1);
            cart.AddItem(Product2, 1);
            cart.AddItem(Product1, 10);

            var results = cart.Lines.OrderBy(line => line.Product.ProductID).ToArray();

            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            // Arrange
            var cart = new Cart();

            cart.AddItem(Product1, 1);
            cart.AddItem(Product2, 3);
            cart.AddItem(Product3, 5);
            cart.AddItem(Product2, 1);

            // Act
            cart.RemoveLine(Product2);

            // Assert
            Assert.Equal(0, cart.Lines.Where(line => line.Product == Product2).Count());
            Assert.Equal(2, cart.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(Product1, 1);
            cart.AddItem(Product2, 1);
            cart.AddItem(Product1, 3);
            var result = cart.ComputeTotalValue();

            // Assert
            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            // Arrange
            var cart = new Cart();

            cart.AddItem(Product1, 1);
            cart.AddItem(Product2, 1);

            // Act
            cart.Clear();

            // Assert
            Assert.Equal(0, cart.Lines.Count());
        }

        private Product Product1 { get; } = new Product { ProductID = 1, Name = "P1", Price = 100M };
        private Product Product2 { get; } = new Product { ProductID = 2, Name = "P2", Price = 50M };
        private Product Product3 { get; } = new Product { ProductID = 3, Name = "P3", Price = 40M };
    }
}
