using Microsoft.EntityFrameworkCore;

namespace TestingApp
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : this(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options
        )
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Employee>()
        //        .Property(e => e.Country)
        //        .IsConcurrencyToken();
        //}
    }
}
