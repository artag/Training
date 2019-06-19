using ProducingResponse.Controllers;
using Xunit;

namespace ProducingResponse.Tests
{
    public class SendingViewBagTests
    {
        [Fact]
        public void ModelObjectType()
        {
            // Arrange
            var controller = new ViewBagController();

            // Act
            var result = controller.SendViewBagToView();

            // Assert
            Assert.IsType<string>(result.ViewData["Message"]);
            Assert.Equal("Hello", result.ViewData["Message"]);
            Assert.IsType<System.DateTime>(result.ViewData["Date"]);
        }
    }
}
