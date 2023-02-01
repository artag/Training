namespace ProductCatalog;

public record Money();
public record ProductCatalogProduct(int ProductId, string ProductName, string Description, Money Price);

public interface IProductStore
{
    IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds);
}

public class ProductStore : IProductStore
{
    public IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds) =>
        productIds.Select(id => new ProductCatalogProduct(id, "foo" + id, "bar", new Money()));
}
