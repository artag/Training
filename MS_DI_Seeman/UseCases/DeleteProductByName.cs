using Common;
using Repository.Interfaces;

namespace UseCases;

public class DeleteProductByName
{
    private readonly IService<DeleteProductRequest, DeleteProductResponse> _deleteProduct;

    public DeleteProductByName(
        IService<DeleteProductRequest, DeleteProductResponse> deleteProduct)
    {
        _deleteProduct = deleteProduct;
    }

    public async Task<Result> Do(string name)
    {
        var request = new DeleteProductRequest(name);
        var response = await _deleteProduct.Execute(request);
        if (response.IsFailure)
            return Result.Failure(new FindProductError(
                $"Product '{name}' not deleted."));

        return Result.Success();
    }
}