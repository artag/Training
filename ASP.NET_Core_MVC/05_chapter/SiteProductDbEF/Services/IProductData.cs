using SiteProduct.Models;

namespace SiteProduct.Services;

public interface IProductData
{
    IEnumerable<Product> GetAll();
    Product Get(int id);
    int Add(Product product);
    void Save(Product product);
    void Delete(Product product);
}
