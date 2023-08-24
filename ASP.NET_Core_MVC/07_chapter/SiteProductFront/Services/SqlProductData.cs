using SiteProduct.Db;
using SiteProduct.Models;

namespace SiteProduct.Services;

public class SqlProductData : IProductData
{
    private readonly Repository _db;

    public SqlProductData(Repository db)
    {
        _db = db;
    }

    public IEnumerable<Product> GetAll()
    {
        _db.ChangeTracker.AutoDetectChangesEnabled = false;
        return _db.Products.ToArray();
    }

    public Product Get(int id)
    {
        _db.ChangeTracker.AutoDetectChangesEnabled = false;
        return _db.Find<Product>(id) ?? new Product { Id = -1 };
    }

    public int Add(Product newProduct)
    {
        _db.Add(newProduct);
        _db.SaveChanges();
        return newProduct.Id;
    }

    public void Save(Product product)
    {
        _db.Update(product);
        _db.SaveChanges();
    }

    public void Delete(Product product)
    {
        _db.Remove(product);
        _db.SaveChanges();
    }
}