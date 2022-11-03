using MediatR;
using Mobile.UseCases.Order.Dto;

namespace Mobile.UseCases.Order.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<int>
{
    public CreateOrderDto Dto { get; set; }
}
