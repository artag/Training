using Common;

namespace Repository.Interfaces;

public record GetAllProductsError(string Message) : Error(Message, null);

public record FindProductError(string Message) : Error(Message, null);

public record DeleteProductError(string Message) : Error(Message, null);
