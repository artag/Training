using System.Linq;
using Domain.Models;
using DomainServices.Interfaces;

namespace DomainServices.Implementation
{
    public class OrderDomainService : IOrderDomainService
    {
        public decimal GetTotal(Order order)
        {
            return order.Items.Sum(x => x.Quantity * x.Product.Price);
        }
    }
}
