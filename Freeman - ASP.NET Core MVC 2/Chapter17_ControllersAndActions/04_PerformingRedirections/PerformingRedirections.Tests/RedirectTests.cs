using PerformingRedirections.Controllers;
using Xunit;

namespace PerformingRedirections.Tests
{
    public class RedirectTests
    {
        [Fact]
        public void Redirection()
        {
            // Arrange
            var controller = new RedirectionController();

            // Act
            var result = controller.Redirect();

            // Assert
            Assert.Equal("/Redirection/LiteralUrl", result.Url);
            Assert.False(result.Permanent);
        }
    }
}