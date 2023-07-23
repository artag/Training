using SiteProduct.Models;

namespace SiteProduct.Services;

public class MockProductData : IProductData
{
    private readonly List<Product> _products = new List<Product>
    {
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
        }
    };

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public Product Get(int id)
    {
        return _products.FirstOrDefault(product => product.Id.Equals(id))
            ?? new Product() { Id = -1 };
    }

    public int Add(Product product)
    {
        var id = _products.Max(p => p.Id) + 1;
        var newProduct = product.WithId(id);
        _products.Add(newProduct);
        return id;
    }

    public void Save(Product product)
    {
        for (var i = 0; i < _products.Count; i++)
        {
            var p = _products[i];
            if (p.Id.Equals(product.Id))
                _products[i] = p.WithProduct(product);
        }
    }
}
