using SiteProduct.Db;
using SiteProduct.Models;

namespace SiteProduct.Services;

public class SqlProductTypeData : IProductTypeData
{
    private readonly Repository _db;

    public SqlProductTypeData(Repository db)
    {
        _db = db;
    }

    public IEnumerable<ProductType> GetAll()
    {
        _db.ChangeTracker.AutoDetectChangesEnabled = false;
        return _db.ProductTypes.ToArray();
    }
}
