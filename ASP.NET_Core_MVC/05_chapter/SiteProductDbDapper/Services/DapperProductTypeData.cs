using System.Data.SQLite;
using Dapper;
using SiteProduct.Models;

namespace SiteProduct.Services;

public class DapperProductTypeData : IProductTypeData
{
    private readonly string _connection;

    public DapperProductTypeData(IConnectionStringProvider provider)
    {
        _connection = provider.GetConnectionString();
    }

    public IEnumerable<ProductType> GetAll()
    {
        using var db = new SQLiteConnection(_connection);
        var query = "SELECT * FROM ProductTypes";
        var types = db.Query<ProductType>(query).ToArray();
        return types;
    }
}