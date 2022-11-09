using AutoMapper;
using DataAccess.Interfaces;
using Delivery.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCases.Order.Dto;

namespace UseCases.Order.Queries.GetById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IDeliveryService _deliveryService;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(
        IDbContext dbContext,
        IDeliveryService deliveryService,
        IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _deliveryService = deliveryService;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items).ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == query.Id);

        if (order == null)
            throw new EntityNotFoundException();

        var dto = _mapper.Map<OrderDto>(order);
        var totalWeight = order.Items.Sum(x => x.Product.Weight);
        var deliveryCost = _deliveryService.CalculateDeliveryCost(totalWeight);
        dto.Total = order.GetTotal() + deliveryCost;

        return dto;
    }
}
