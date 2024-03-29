using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Model.EntityConfigurations;

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

            // Эта строка применит все объекты типа `IEntityTypeConfiguration<T>`,
            // существующие в текущей сборке.
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<ExpenseLine>()
                .Property(e => e.TotalCost)
                .HasComputedColumnSql("[Quantity] * [UnitCost]");

            // Конфигурация перенесена в класс "UserConfiguration".
            // builder.Entity<User>()
            //     .Property(p => p.FullName)
            //     .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

            // Применение файла конфигурации "UserConfiguration".
            // Заменено строкой выше: builder.ApplyConfigurationsFromAssembly(...)
            // builder.ApplyConfiguration(new UserConfiguration());

            builder.Entity<ExpenseHeader>()
                .Property(e => e.UsdExchangeRate)
                .HasColumnType("decimal(13,4)")
                .IsRequired(true);

            builder.Entity<ExpenseHeader>()
                .HasOne(e => e.Requester)
                .WithMany(e => e.RequesterExpenseHeaders)
                .HasForeignKey(e => e.RequesterId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Filename=EFExpenseDemo.db");
        }
    }
}