using System.Collections.Concurrent;

namespace ProductCatalog;

public record Money(string Currency, decimal Amount);
public record ProductCatalogProduct(int ProductId, string ProductName, string ProductDescription, Money Price);

public interface IProductStore
{
    IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds);
}

public class ProductStore : IProductStore
{
    public readonly ConcurrentDictionary<int, ProductCatalogProduct> _catalog = new();

    public IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds)
    {
        return productIds
            .Select(id => _catalog.GetOrAdd(id, _ => CreateNewProduct(id)));
    }

    private ProductCatalogProduct CreateNewProduct(int id)
    {
        var name = CreateName();
        var desc = CreateDescription(name);
        var price = CreatePrice();
        return new ProductCatalogProduct(id, name, desc, price);
    }

    private string CreateName()
    {
        var names = new[]
        {
            "t-shirt", "shirt", "shoes", "coat", "dress", "hat", "jeans", "trousers", "shorts", "socks"
        };
        var i = new Random().Next(0, names.Length);
        return names[i];
    }

    private string CreateDescription(string name)
    {
        var desc = new[] { "blue", "red", "green", "nice", "short", "beautiful", "yellow", "black" };
        var i = new Random().Next(0, desc.Length);
        return $"{desc[i]} {name}";
    }

    private Money CreatePrice()
    {
        var cur = new[] { "eur", "usd", "gbp", "cad" };
        var i = new Random().Next(0, cur.Length);
        var amount = new Random().Next(10, 500);
        return new Money(cur[i], amount);
    }
}
