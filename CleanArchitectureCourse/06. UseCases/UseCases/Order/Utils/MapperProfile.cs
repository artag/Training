using AutoMapper;
using Domain.Entities;
using UseCases.Order.Dto;

namespace UseCases.Order.Utils;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Domain.Entities.Order, OrderDto>();
        CreateMap<CreateOrderDto, Domain.Entities.Order>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
