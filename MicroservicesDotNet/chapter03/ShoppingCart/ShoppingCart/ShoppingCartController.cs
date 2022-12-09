using Microsoft.AspNetCore.Mvc;
using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart;

// (1) Tells MVC that all routes in this controller start with '/shoppingcart'
// (2) Declares ShoppingCartController as a controller

// (3) Declares the endpoint for handling requests to /shoppingcart/{userid},
//     such as '/shoppingcart/123'
// (4) Assigns the {userId} from the URL to the userId variable
// (5) Returns the user’s shopping cart.
//     MVC serializes it before sending it to the client.

// (6) Declares an HTTP POST endpoint for /shoppingcart/{userid}/item
// (7) Reads and deserializes the array of product IDs in the HTTP request body
// (8) Fetches the product information from the Product Catalog microservice
// (9) Adds items to the cart
// (10) Saves the updated cart to the data store
// (11) Returns the updated cart

// (12) Using the same route template for two route declarations is fine
//      if they use different HTTP methods.
// (13) The eventStore will be used later in the RemoveItems method.

[Route("/shoppingcart")]                                // (1)
public class ShoppingCartController : ControllerBase    // (2)
{
    private readonly IShoppingCartStore _shoppingCartStore;
    private readonly IProductCatalogClient _productCatalogClient;
    private readonly IEventStore _eventStore;

    public ShoppingCartController(
        IShoppingCartStore shoppingCartStore,
        IProductCatalogClient productCatalogClient,
        IEventStore eventStore)
    {
        _shoppingCartStore = shoppingCartStore;
        _productCatalogClient = productCatalogClient;
        _eventStore = eventStore;
    }

    [HttpGet("{userId:int}")]                   // (3)
    public ShoppingCart Get(int userId) =>      // (4)
        _shoppingCartStore.Get(userId);         // (5)

    [HttpPost("{userId:int}/items")]                                                    // (6)
    public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)       // (7)
    {
        var cart = _shoppingCartStore.Get(userId);
        var cartItems = await _productCatalogClient.GetShoppingCartItems(productIds);   // (8)
        cart.AddItems(cartItems, _eventStore);                                          // (9)
        _shoppingCartStore.Save(cart);                                                  // (10)
        return cart;                                                                    // (11)
    }

    [HttpDelete("{userId:int}/items")]                                      // (12)
    public ShoppingCart Delete(int userId, [FromBody] int[] productIds)
    {
        var cart = _shoppingCartStore.Get(userId);
        cart.RemoveItems(productIds, _eventStore);                          // (13)
        _shoppingCartStore.Save(cart);
        return cart;
    }
}
