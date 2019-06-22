using ReturningDifferentContent.Controllers;
using Xunit;

namespace ReturningDifferentContent.Tests
{
    public class StatusCodeTests
    {
        [Fact]
        public void NotFoundActionMethod()
        {
            // Arrange
            var controller = new StatusCodeController();

            // Act
            var result = controller.MuchBetter();

            // Assert
            Assert.Equal(404, result.StatusCode);
        }
    }
}
