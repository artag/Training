namespace ShoppingCart.ShoppingCart;

// (1) Simple data access interface
// (2) Use an in-memory dictionary instead of a database for now.

public interface IShoppingCartStore     // (1)
{
    ShoppingCart Get(int userId);
    void Save(ShoppingCart shoppingCart);
}

public class ShoppingCartStore : IShoppingCartStore
{
    private static readonly Dictionary<int, ShoppingCart> Database = new();     // (2)

    public ShoppingCart Get(int userId) =>
        Database.ContainsKey(userId)
            ? Database[userId]
            : new ShoppingCart(userId);

    public void Save(ShoppingCart shoppingCart) =>
        Database[shoppingCart.UserId] = shoppingCart;
}
