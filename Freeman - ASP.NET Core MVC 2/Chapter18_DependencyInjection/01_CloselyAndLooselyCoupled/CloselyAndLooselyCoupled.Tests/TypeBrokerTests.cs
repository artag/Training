using CloselyAndLooselyCoupled.Controllers;
using CloselyAndLooselyCoupled.Infrastructure;
using CloselyAndLooselyCoupled.Models;
using Moq;
using Xunit;

namespace CloselyAndLooselyCoupled.Tests
{
    public class TypeBrokerTests
    {
        [Fact]
        public void ControllerTests()
        {
            // Arrange
            var data = new[] { new Product { Name = "Test", Price = 100M } };

            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Products).Returns(data);

            TypeBroker.SetTestObject(mock.Object);
            var controller = new UsageTypeBrokerController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.Equal(data, result.ViewData.Model);
        }
    }
}
