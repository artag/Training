using SiteProduct.Models;

namespace SiteProduct.Services;

public class MockProductData : IProductData
{
    private readonly IReadOnlyCollection<Product> _products = new[]
    {
        new Product
        {
            Id = 1,
            Name = "Шилдт Г. C# 4.0: Полное руководство.",
            Price = 750.0M,
            ProductionDate = new DateTime(2019, 03, 01)
        },
        new Product
        {
            Id = 2,
            Name = "Оперативная память Kingston RAM 1x4 ГБ DDR4",
            Price = 1975.0M,
            ProductionDate = new DateTime(2021, 05, 01),
        },
        new Product
        {
            Id = 3,
            Name = "Apple iPhone SE 64GB",
            Price = 34789.0M,
            ProductionDate = new DateTime(2020, 12, 01),
        }
    };

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }
}
