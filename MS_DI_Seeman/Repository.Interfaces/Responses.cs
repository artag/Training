using Domain;

namespace Repository.Interfaces;

public record GetProductsResponse(IReadOnlyCollection<Product> Products);

public record FindProductResponse(Product Product);

public record DeleteProductResponse();
