using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart;

// (1) ASP.NET injects an HttpClient.
// (2) Configure the HttpClient to use the base address of the product catalog.
// (3) Configure the HttpClient to accept JSON responses from the product catalog.
// (4) Tells HttpClient to perform the HTTP GET asynchronously

// (5) Uses System.Text.Json to deserialize the JSON from the product catalog microservice
// (6) Creates a ShoppingCartItem for each product in the response
// (7) Uses a private record to represent the product data

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
}

public class ProductCatalogClient : IProductCatalogClient
{
    // URL of the fake product catalog microservice
    private static readonly string ProductCatalogBaseUrl = @"https://git.io/JeHiE";
    private static readonly string GetProductPathTemplate = "?productIds=[{0}]";

    private readonly HttpClient _client;

    public ProductCatalogClient(HttpClient client)              // (1)
    {
        client.BaseAddress = new Uri(ProductCatalogBaseUrl);    // (2)
        client.DefaultRequestHeaders.Accept                     // (3)
            .Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        _client = client;
    }

    // Fetching products and converting them to shopping cart items
    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(
        int[] productCatalogIds)
    {
        using var response = await RequestProductFromProductCatalog(productCatalogIds);
        return await ConvertToShoppingCartItems(response);
    }

    private Task<HttpResponseMessage> RequestProductFromProductCatalog(
        int[] productCatalogIds)
    {
        var productsResource = string.Format(
            GetProductPathTemplate,
            string.Join(",", productCatalogIds));

        return _client.GetAsync(productsResource);  // (4)
    }

    private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(
        HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var products =
            await JsonSerializer.DeserializeAsync<List<ProductCatalogProduct>>(stream, options)
            ?? new List<ProductCatalogProduct>();   // (5)

        return products.Select(p =>
            new ShoppingCartItem(                   // (6)
                p.ProductId,
                p.ProductName,
                p.ProductDescription,
                p.Price));
    }

    private record ProductCatalogProduct(           // (7)
        int ProductId, string ProductName, string ProductDescription, Money Price);
}
