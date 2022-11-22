using DomainServices.Interfaces;
using Northwind.Domain.Entities;

namespace DomainServices
{
    public class OrderDomainService : IOrderDomainService
    {
        public decimal GetPrice(Order order, TransportInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
