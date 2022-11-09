using AutoMapper;
using Domain.Models;
using UseCases.Order.Dto;

namespace UseCases.Order.Utils;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Domain.Models.Order, OrderDto>();
        CreateMap<CreateOrderDto, Domain.Models.Order>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
