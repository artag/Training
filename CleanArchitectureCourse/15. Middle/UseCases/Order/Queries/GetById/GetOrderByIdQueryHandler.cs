using AutoMapper;
using DataAccess.Interfaces;
using Delivery.Interfaces;
using DomainServices.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCases.Order.Dto;

namespace UseCases.Order.Queries.GetById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IDbContext _dbContext;
    private readonly IDeliveryService _deliveryService;
    private readonly IOrderDomainService _domainService;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(
        IDbContext dbContext,
        IMapper mapper,
        IDeliveryService deliveryService,
        IOrderDomainService domainService)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _deliveryService = deliveryService;
        _domainService = domainService;
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
        dto.Total = _domainService.GetTotal(order, _deliveryService.CalculateDeliveryCost);
        return dto;
    }
}
