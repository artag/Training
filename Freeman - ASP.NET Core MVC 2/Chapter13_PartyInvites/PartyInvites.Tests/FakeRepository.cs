using System.Collections.Generic;
using PartyInvites.Models;

namespace PartyInvites.Tests
{
    public class FakeRepository : IRepository
    {
        public IEnumerable<GuestResponse> Responses =>
            new List<GuestResponse>
            {
                new GuestResponse { Name = "Bob", WillAttend = true },
                new GuestResponse { Name = "Alice", WillAttend = true },
                new GuestResponse { Name = "Joe", WillAttend = false }
            };

        public void AddResponse(GuestResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}