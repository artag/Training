using AutoMapper;
using Domain.Entities;
using Mobile.UseCases.Order.Dto;

namespace Mobile.UseCases.Order.Utils;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Domain.Entities.Order, OrderDto>();
        CreateMap<CreateOrderDto, Domain.Entities.Order>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
