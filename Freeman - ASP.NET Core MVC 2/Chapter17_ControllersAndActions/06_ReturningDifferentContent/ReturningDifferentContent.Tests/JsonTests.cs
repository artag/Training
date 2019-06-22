using ReturningDifferentContent.Controllers;
using Xunit;

namespace ReturningDifferentContent.Tests
{
    public class JsonTests
    {
        [Fact]
        public void JsonActionMethod()
        {
            // Arrange
            var controller = new JsonController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.Equal(new[] { "Alice", "Bob", "Joe" }, result.Value);
        }
    }
}
