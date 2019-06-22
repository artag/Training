using IntroducingDI.Controllers;
using IntroducingDI.Models;
using Moq;
using Xunit;

namespace IntroducingDI.Tests
{
    public class DIControllerTests
    {
        [Fact]
        public void ControllerTest()
        {
            // Arrange
            var data = new[] { new Product { Name = "Test", Price = 100M } };

            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Products).Returns(data);

            var controller = new InjectionController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.Equal(data, result.ViewData.Model);
        }
    }
}
