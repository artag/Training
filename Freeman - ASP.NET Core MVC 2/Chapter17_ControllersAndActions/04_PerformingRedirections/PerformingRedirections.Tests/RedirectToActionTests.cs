using PerformingRedirections.Controllers;
using Xunit;

namespace PerformingRedirections.Tests
{
    public class RedirectToActionTests
    {
        [Fact]
        public void Redirection()
        {
            // Arrange
            var controller = new RedirectionController();

            // Act
            var result = controller.ActionRedirectToAction();

            // Assert
            Assert.False(result.Permanent);
            Assert.Equal("ActionRedirection", result.ActionName);
        }
    }
}
