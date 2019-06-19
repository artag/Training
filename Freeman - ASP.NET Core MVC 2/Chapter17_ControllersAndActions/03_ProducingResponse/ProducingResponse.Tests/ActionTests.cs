using ProducingResponse.Controllers;
using Xunit;

namespace ProducingResponse.Tests
{
    public class ActionTests
    {
        [Fact]
        public void ViewSelected()
        {
            // Arrange
            var controller = new ResponseController();

            // Act
            var result = controller.SendViewResult("Adam", "London");

            // Assert
            Assert.Equal("Result", result.ViewName);
        }

        [Fact]
        public void DefaultView()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.Null(result.ViewName);
        }
    }
}
