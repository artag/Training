using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using WebApp.Interfaces;

namespace Mobile.UseCases.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IMapper _mapper;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrderCommandHandler(
        IMapper mapper,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Domain.Models.Order>(command.Dto);
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        return order.Id;
    }
}
