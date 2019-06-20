using PerformingRedirections.Controllers;
using Xunit;

namespace PerformingRedirections.Tests
{
    public class RedirectToRouteTests
    {
        [Fact]
        public void Redirect()
        {
            // Arrange
            var controller = new RedirectionController();

            // Act
            var result = controller.RedirectToRoute();

            // Assert
            Assert.False(result.Permanent);
            Assert.Equal("Redirection", result.RouteValues["controller"]);
            Assert.Equal("RoutedRedirection", result.RouteValues["action"]);
            Assert.Equal("MyID", result.RouteValues["id"]);
        }
    }
}