using Common;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;

namespace UseCases;

public class ListAllProducts : IDisposable
{
    private readonly string _uid = Guid.NewGuid().ToString("D").Substring(0, 8);
    private readonly IService<GetProductsRequest, GetProductsResponse> _getProductsRequest;
    private readonly ILogger<ListAllProducts> _log;

    public ListAllProducts(
        IService<GetProductsRequest, GetProductsResponse> getProductsRequest,
        ILogger<ListAllProducts> log)
    {
        _getProductsRequest = getProductsRequest ?? throw new ArgumentNullException();
        _log = log ?? throw new ArgumentNullException();
        _log.LogInformation("Ctor '{0}', uid = '{1}'", nameof(ListAllProducts), _uid);
    }

    public async Task<Result> Do()
    {
        var request = new GetProductsRequest();
        var response = await _getProductsRequest.Execute(request);
        if (response.IsFailure)
            return Result.Failure(new GetAllProductsError("Can't get products from repository"));

        var products = response.Value!.Products;
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }

        return Result.Success();
    }

    public void Dispose()
    {
        _log.LogInformation("Dispose '{0}', uid = '{1}'", nameof(ListAllProducts), _uid);
    }
}