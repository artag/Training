using System.Collections.Generic;

namespace PartyInvites.Models
{
    public class EFRepository : IRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<GuestResponse> Responses => _context.Invites;

        public void AddResponse(GuestResponse response)
        {
            _context.Invites.Add(response);
            _context.SaveChanges();
        }
    }
}