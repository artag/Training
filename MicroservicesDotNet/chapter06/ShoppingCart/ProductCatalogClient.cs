using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
}

public class ProductCatalogClient : IProductCatalogClient
{
    // URL of the fake product catalog microservice. Remote service.
    //private static readonly string ProductCatalogBaseUrl = @"https://git.io/JeHiE";

    // My product catalog microservice.
    private static readonly string ProductCatalogBaseUrl = @"https://localhost:5003/products";

    private static readonly string GetProductPathTemplate = "?productIds=[{0}]";

    private readonly HttpClient _client;
    private readonly ICache _cache;

    // (1) Configure the HttpClient to use the base address of the product catalog.
    // (2) Configure the HttpClient to accept JSON responses from the product catalog.
    public ProductCatalogClient(HttpClient client, ICache cache)
    {
        client.BaseAddress = new Uri(ProductCatalogBaseUrl);    // (1)
        client.DefaultRequestHeaders.Accept                     // (2)
            .Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        _client = client;
        _cache = cache;
    }

    // Fetching products and converting them to shopping cart items
    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(
        int[] productCatalogIds)
    {
        using var response = await RequestProductFromProductCatalog(productCatalogIds);
        return await ConvertToShoppingCartItems(response);
    }

    // (3) Tries to retrieve a valid response from the cache.
    // (4) Only makes the HTTP request if there’s no response in the cache.
    // (5) Tells HttpClient to perform the HTTP GET asynchronously.
    private async Task<HttpResponseMessage> RequestProductFromProductCatalog(
        int[] productCatalogIds)
    {
        var productsResource = string.Format(
            GetProductPathTemplate,
            string.Join(",", productCatalogIds));
        var response = _cache.Get(productsResource) as HttpResponseMessage;     // (3)
        if (response is null)                                                   // (4)
        {
            response = await _client.GetAsync(productsResource);                // (5)
            AddToCache(productsResource, response);
        }

        return response;
    }

    // (6) Reads the cache-control header from the response.
    // (7) Parses the cache-control value and extracts max-age from it.
    // (8) Adds the response to the cache if it has a max-age value.
    private void AddToCache(string resource, HttpResponseMessage response)
    {
        var cacheHeader = response.Headers
            .FirstOrDefault(h => h.Key == "cache-control");                 // (6)
        if (!string.IsNullOrEmpty(cacheHeader.Key)
            && CacheControlHeaderValue.TryParse(                            // (7)
                cacheHeader.Value.ToString(), out var cacheControl)
            && cacheControl?.MaxAge.HasValue is true)
            _cache.Add(resource, response, cacheControl.MaxAge!.Value);     // (8)
    }

    // (9) Uses System.Text.Json to deserialize the JSON from the product catalog microservice.
    // (10) Creates a ShoppingCartItem for each product in the response.
    // (11) Uses a private record to represent the product data.
    private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(
        HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var products =
            await JsonSerializer.DeserializeAsync<List<ProductCatalogProduct>>(stream, options)
            ?? new List<ProductCatalogProduct>();   // (9)

        return products.Select(p =>
            new ShoppingCartItem(                   // (10)
                p.ProductId,
                p.ProductName,
                p.ProductDescription,
                p.Price));
    }

    private record ProductCatalogProduct(           // (11)
        int ProductId, string ProductName, string ProductDescription, Money Price);
}
