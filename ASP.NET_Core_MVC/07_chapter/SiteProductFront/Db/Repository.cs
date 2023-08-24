using Microsoft.EntityFrameworkCore;
using SiteProduct.Models;

namespace SiteProduct.Db;

public sealed class Repository : DbContext
{
    public Repository(DbContextOptions<Repository> options) : base(options)
    {
        // Создание базы данных при первом обращении.
        Database.EnsureCreated();
    }

    /// <summary>
    /// Товары.
    /// </summary>
    /// <remarks>
    /// null! - переменные, которые не допускают значений null, должны быть проинициализированы перед использованием.
    /// </remarks>
    public DbSet<Product> Products { get; set; } = null!;

    /// <summary>
    /// Категории товара.
    /// </summary>
    /// <remarks>
    /// null! - переменные, которые не допускают значений null, должны быть проинициализированы перед использованием.
    /// </remarks>
    public DbSet<ProductType> ProductTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Добавление категорий товаров, если их нет.
        modelBuilder.Entity<ProductType>().HasData(
            new ProductType { Id = 1, TypeName = "Прочие" },
            new ProductType { Id = 2, TypeName = "Книги" },
            new ProductType { Id = 3, TypeName = "Комплектующие для компьютеров" },
            new ProductType { Id = 4, TypeName = "Смартфоны" });

        // Товары для тестирования, чтобы лишний раз не добавлять.
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Шилдт Г. C# 4.0: Полное руководство.",
                Price = 750.0M,
                ProductionDate = new DateTime(2019, 03, 01),
                CategoryId = 2,
            },
            new Product
            {
                Id = 2,
                Name = "Оперативная память Kingston RAM 1x4 ГБ DDR4",
                Price = 1975.0M,
                ProductionDate = new DateTime(2021, 05, 01),
                CategoryId = 3,
            },
            new Product
            {
                Id = 3,
                Name = "Apple iPhone SE 64GB",
                Price = 34789.0M,
                ProductionDate = new DateTime(2020, 12, 01),
                CategoryId = 4,
            });
    }
}
