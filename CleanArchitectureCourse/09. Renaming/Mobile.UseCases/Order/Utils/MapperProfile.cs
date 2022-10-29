using AutoMapper;
using Domain.Models;
using Mobile.UseCases.Order.Dto;

namespace Mobile.UseCases.Order.Utils;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Domain.Models.Order, OrderDto>();
        CreateMap<CreateOrderDto, Domain.Models.Order>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
