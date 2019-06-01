using System.Collections.Generic;
using System.Linq;
using PartyInvites.Controllers;
using PartyInvites.Models;
using Xunit;

namespace PartyInvites.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void ListActionFiltersNonAttendees()
        {
            // Assign
            var controller = new HomeController(new FakeRepository());
            
            // Act
            var result = controller.ListResponses().Model as IEnumerable<GuestResponse>;
            
            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}