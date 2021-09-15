using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FullName)
                .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

            // Генерация даты при создании новой записи в таблице. Для MSSQL.
            // Для SQLite не получилось сделать.
            // builder.Property(u => u.CreatedDate).ValueGeneratedOnAdd()
            //     .HasDefaultValueSql("GETUTCDATE()");

            // Генерация даты при добавлении/обновлении записи в таблице. Для MSSQL.
            // Для SQLite не получилось сделать.
            // builder.Property(u => u.LastModified).ValueGeneratedOnAddOrUpdate()
            //     .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
