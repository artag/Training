using ProducingResponse.Controllers;
using Xunit;

namespace ProducingResponse.Tests
{
    public class SendingModelTests
    {
        [Fact]
        public void ModelObjectType()
        {
            // Arrange
            var controller = new ModelController();

            // Act
            var result = controller.SendToTypedView();

            // Assert
            Assert.IsType<System.DateTime>(result.ViewData.Model);
        }
    }
}
