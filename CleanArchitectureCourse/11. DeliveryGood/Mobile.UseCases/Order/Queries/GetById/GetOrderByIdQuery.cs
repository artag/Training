using MediatR;
using Mobile.UseCases.Order.Dto;

namespace Mobile.UseCases.Order.Queries.GetById;

public class GetOrderByIdQuery : IRequest<OrderDto>
{
    public int Id { get; set; }
}
