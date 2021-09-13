using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ExpenseHeader> ExpenseHeaders { get; set; }
        public DbSet<ExpenseLine> ExpenseLines { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExpenseLine>()
                .Property(e => e.TotalCost)
                .HasComputedColumnSql("[Quantity] * [UnitCost]");

            builder.Entity<User>()
                .Property(p => p.FullName)
                .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

            builder.Entity<ExpenseHeader>()
                .Property(e => e.UsdExchangeRate)
                .HasColumnType("decimal(13,4)")
                .IsRequired(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Filename=EFExpenseDemo.db");
        }
    }
}