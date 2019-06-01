using Microsoft.EntityFrameworkCore;

namespace PartyInvites.Models
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=PartyInvites.db");
        }

        public DbSet<GuestResponse> Invites { get; set; }
    }
}