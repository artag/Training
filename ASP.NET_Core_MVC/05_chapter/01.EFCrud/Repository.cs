using EFCrud.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCrud;

public class Repository : DbContext
{
    public Repository()
    {
        // Создание DbSet<TEntity>, который можно использовать для запроса
        // и сохранения экземпляров MobileDevice.
        MobileDevices = Set<MobileDevice>();
    }

    public DbSet<MobileDevice> MobileDevices { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=MobileDevices.db");
    }
}
