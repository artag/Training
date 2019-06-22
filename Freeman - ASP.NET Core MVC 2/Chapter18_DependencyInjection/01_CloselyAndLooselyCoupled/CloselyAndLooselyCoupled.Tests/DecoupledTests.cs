using CloselyAndLooselyCoupled.Controllers;
using CloselyAndLooselyCoupled.Models;
using Moq;
using Xunit;

namespace CloselyAndLooselyCoupled.Tests
{
    public class DecoupledTests
    {
        [Fact]
        public void ControllerTest()
        {
            // Arrange
            var data = new[] { new Product { Name = "Test", Price = 100M } };

            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Products).Returns(data);

            var controller = new DecoupledController();
            controller.Repository = mock.Object;

            // Act
            var result = controller.Index();

            // Assert
            Assert.Equal(data, result.ViewData.Model);
        }
    }
}
