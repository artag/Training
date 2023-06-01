using Domain;

namespace Repository.Interfaces;

public record GetProductsRequest();

public record FindProductRequest(string Name);

public record DeleteProductRequest(string Name);
