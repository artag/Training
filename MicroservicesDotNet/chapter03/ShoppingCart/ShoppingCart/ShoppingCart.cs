using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart;

// (0) Raises an event through the eventStore for each item

public class ShoppingCart
{
    private readonly HashSet<ShoppingCartItem> _items = new();

    public ShoppingCart(int userId) =>
        UserId = userId;

    public IEnumerable<ShoppingCartItem> Items => _items;

    public int UserId { get; }

    public void AddItems(
        IEnumerable<ShoppingCartItem> shoppingCartItems,
        IEventStore eventStore)
    {
        foreach (var item in shoppingCartItems)
            if (_items.Add(item))
                eventStore.Raise("ShoppingCartItemAdded", new { UserId, item });   // (0)
    }

    public void RemoveItems(int[] productCatalogIds, IEventStore eventStore) =>
        _items.RemoveWhere(i => productCatalogIds.Contains(i.ProductCatalogId));
}

public record ShoppingCartItem(
    int ProductCatalogId,
    string ProductName,
    string Description,
    Money Price)
{
    public virtual bool Equals(ShoppingCartItem? obj) =>
        obj != null && ProductCatalogId.Equals(obj.ProductCatalogId);

    public override int GetHashCode() =>
        ProductCatalogId.GetHashCode();
}

public record Money(string Currency, decimal Amount);
