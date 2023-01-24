using System.Data.SqlClient;
using Dapper;

namespace ShoppingCart.ShoppingCart;

public interface IShoppingCartStore
{
    Task<ShoppingCart> Get(int userId);
    Task Save(ShoppingCart shoppingCart);
}

// (1) - Connection string to the ShoppingCart database in the MSSQL Docker container.
// (2) - Dapper expects and allows you to write your own SQL.
// (3) - Opens a connection to the ShoppingCart database.
// (4) - Uses a Dapper extension method to execute a SQL query.
// (5) - The result set from the SQL query to ShoppingCartItem.
// (6) - Create a row in the ShoppingCart table
//       if the shopping cart does not already have an Id.
// (7) - Deletes all pre-existing shopping cart items.
// (8) - Adds the current shopping cart items.
// (9) - Commits all changes to the database.
public class ShoppingCartStore : IShoppingCartStore
{
    // (1)
    private const string ConnectionString =
        @"Data Source=localhost;Initial Catalog=ShoppingCart;User Id=SA; Password=Some_password!";

    // (2)
    private const string ReadItemsSql = @"
SELECT ShoppingCart.Id, ProductCatalogId, ProductName, ProductDescription, Currency, Amount
FROM ShoppingCart, ShoppingCartItem
WHERE ShoppingCartItem.ShoppingCartId = ShoppingCart.ID
AND ShoppingCart.UserId = @UserId";

    private const string InsertShoppingCartSql = @"
INSERT INTO ShoppingCart (UserId) OUTPUT inserted.ID VALUES (@UserId)";

    private const string DeleteAllForShoppingCartSql = @"
DELETE item FROM ShoppingCartItem item
INNER JOIN ShoppingCart cart ON item.ShoppingCartId = cart.ID
AND cart.UserId = @UserId";

    private const string AddAllForShoppingCartSql = @"
INSERT INTO ShoppingCartItem(
    ShoppingCartId, ProductCatalogId, ProductName,
    ProductDescription, Amount, Currency)
VALUES(
    @ShoppingCartId, @ProductCatalogId, @ProductName,
    @ProductDescription, @Amount, @Currency)";

    public async Task<ShoppingCart> Get(int userId)
    {
        await using var conn = new SqlConnection(ConnectionString);     // (3)

        var query = await conn.QueryAsync(
            ReadItemsSql, new { UserId = userId });     // (4)
        var items = query.ToList();

        return new ShoppingCart(
            items.FirstOrDefault()?.ID,
            userId,
            items.Select(x => new ShoppingCartItem(     // (5)
                (int)x.ProductCatalogId,
                x.ProductName,
                x.ProductDescription,
                new Money(x.Currency, x.Amount))));
    }

    public async Task Save(ShoppingCart shoppingCart)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();
        await using (var tx = conn.BeginTransaction())
        {
            var shoppingCartId = shoppingCart.Id        // (6)
                ?? await conn.QuerySingleAsync<int>(
                    InsertShoppingCartSql, new { shoppingCart.UserId }, tx);

            await conn.ExecuteAsync(                    // (7)
                DeleteAllForShoppingCartSql, new { UserId = shoppingCart.UserId }, tx);

            await conn.ExecuteAsync(                    // (8)
                AddAllForShoppingCartSql,
                shoppingCart.Items.Select(x => new
                {
                    shoppingCartId,
                    x.ProductCatalogId,
                    ProductDescription = x.Description,
                    x.ProductName,
                    x.Price.Amount,
                    x.Price.Currency
                }), tx);

            await tx.CommitAsync();                     // (9)
        }
    }
}
