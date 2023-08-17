using System.Data.SQLite;
using Dapper;

namespace SiteProduct.Services;

public class DataSeed : IDataSeed
{
    private readonly string _connection;

    public DataSeed(IConnectionStringProvider provider)
    {
        _connection = provider.GetConnectionString();
    }

    public void EnsureDatabase()
    {
        EnsureProductTypes();
        EnsureProducts();
    }

    private void EnsureProductTypes()
    {
        var exists = CheckTableExists("ProductTypes");
        if (exists)
            return;

        CreateProductTypesTable();
        SeedDataToProductTypesTable();
    }

    private void EnsureProducts()
    {
        var exists = CheckTableExists("Products");
        if (exists)
            return;

        CreateProductsTable();
        SeedDataToProductsTable();
    }

    private bool CheckTableExists(string tableName)
    {
        using var db = new SQLiteConnection(_connection);
        var query = "SELECT 1 " +
                    "FROM sqlite_master " +
                    $"WHERE type='table' AND name='{tableName}'";

        var exists = db.Query<int?>(query).FirstOrDefault() ?? -1;
        return exists == 1;
    }

    private void CreateProductTypesTable()
    {
        using var db = new SQLiteConnection(_connection);
        var createQuery = "CREATE TABLE \"ProductTypes\" (" +
                          "\"Id\" INTEGER NOT NULL CONSTRAINT \"PK_ProductTypes\" PRIMARY KEY AUTOINCREMENT, " +
                          "\"TypeName\" TEXT NOT NULL)";
        db.Execute(createQuery);
    }

    private void CreateProductsTable()
    {
        using var db = new SQLiteConnection(_connection);
        var createQuery = "CREATE TABLE \"Products\" (" +
                          "\"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Products\" PRIMARY KEY AUTOINCREMENT, " +
                          "\"Name\" TEXT NOT NULL, " +
                          "\"Price\" INTEGER NOT NULL, " +
                          "\"ProductionDate\" TEXT NOT NULL, " +
                          "\"CategoryId\" INTEGER NOT NULL)";
        db.Execute(createQuery);
    }

    private void SeedDataToProductTypesTable()
    {
        using var db = new SQLiteConnection(_connection);
        var seedQueries = new[]
        {
            "INSERT INTO ProductTypes (Id, TypeName) VALUES (1, 'Прочие')",
            "INSERT INTO ProductTypes (Id, TypeName) VALUES (2, 'Книги')",
            "INSERT INTO ProductTypes (Id, TypeName) VALUES (3, 'Комплектующие для компьютеров')",
            "INSERT INTO ProductTypes (Id, TypeName) VALUES (4, 'Смартфоны')",
        };

        foreach (var seedQuery in seedQueries)
            db.Execute(seedQuery);
    }

    private void SeedDataToProductsTable()
    {
        using var db = new SQLiteConnection(_connection);
        const string formatDate = "yyyy-MM-dd";
        var dates = new[]
        {
            new DateTime(2019, 03, 01).ToString(formatDate),
            new DateTime(2021, 05, 01).ToString(formatDate),
            new DateTime(2020, 12, 01).ToString(formatDate),
        };

        var seedQueries = new[]
        {
            "INSERT INTO Products (Id, Name, Price, ProductionDate, CategoryId) " +
            $"VALUES (1, 'Шилдт Г. C# 4.0: Полное руководство.', 750.0, '{dates[0]}', 2)",

            "INSERT INTO Products (Id, Name, Price, ProductionDate, CategoryId) " +
            $"VALUES (2, 'Оперативная память Kingston RAM 1x4 ГБ DDR4', 1975.0, '{dates[1]}', 3)",

            "INSERT INTO Products (Id, Name, Price, ProductionDate, CategoryId) " +
            $"VALUES (3, 'Apple iPhone SE 64GB', 9789.0, '{dates[2]}', 4)",
        };

        foreach (var seedQuery in seedQueries)
            db.Execute(seedQuery);
    }
}
