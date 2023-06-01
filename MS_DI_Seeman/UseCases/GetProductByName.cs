using Common;
using Domain;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;

namespace UseCases;

public class GetProductByName
{
    private readonly IService<FindProductRequest, FindProductResponse> _findProduct;
    private readonly ILogger<GetProductByName> _log;

    public GetProductByName(
        IService<FindProductRequest, FindProductResponse> findProduct,
        ILogger<GetProductByName> log)
    {
        _findProduct = findProduct ?? throw new ArgumentNullException(nameof(findProduct));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public async Task<Result<Product>> Do(string name)
    {
        var request = new FindProductRequest(name);
        var response = await _findProduct.Execute(request);
        if (response.IsFailure)
            return Result.Failure<Product>(new FindProductError(
                $"Product with name '{name}' not found in repository."));

        var product = response.Value!.Product;
        return Result.Success(product);
    }
}