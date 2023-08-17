using System.Data.SQLite;
using Dapper;
using SiteProduct.Models;

namespace SiteProduct.Services;

public class DapperProductData : IProductData
{
    private readonly string _connection;

    public DapperProductData(IConnectionStringProvider provider)
    {
        _connection = provider.GetConnectionString();
    }

    public IEnumerable<Product> GetAll()
    {
        using var db = new SQLiteConnection(_connection);
        const string query = "SELECT * FROM Products";
        var products = db.Query<Product>(query).ToArray();
        return products;
    }

    public Product Get(int id)
    {
        using var db = new SQLiteConnection(_connection);
        const string query = "SELECT * FROM Products WHERE Id = @id";
        var product = db.Query<Product>(query, new { id }).FirstOrDefault() ?? new Product() { Id = -1 };
        return product;
    }

    public int Add(Product product)
    {
        using var db = new SQLiteConnection(_connection);
        var query = "INSERT INTO Products (Name, Price, ProductionDate, CategoryId) " +
                    "VALUES (@Name, @Price, @ProductionDate, @CategoryId)";
        var productId = db.Query<int>(query, product).FirstOrDefault();
        product.Id = productId;
        return productId;
    }

    public void Save(Product product)
    {
        using var db = new SQLiteConnection(_connection);
        var query = "UPDATE Products " +
                    "SET Name = @Name, Price = @Price, ProductionDate = @ProductionDate, CategoryId = @CategoryId " +
                    "WHERE Id = @Id";
        db.Execute(query, product);
    }

    public void Delete(Product product)
    {
        using var db = new SQLiteConnection(_connection);
        var query = "DELETE FROM Products WHERE Id = @Id";
        db.Execute(query, new { product.Id });
    }
}