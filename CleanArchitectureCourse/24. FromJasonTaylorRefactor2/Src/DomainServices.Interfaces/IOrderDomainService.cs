using Northwind.Domain.Entities;

namespace DomainServices.Interfaces
{
    public interface IOrderDomainService
    {
        decimal GetPrice(Order order, TransportInfo info);
    }
}
