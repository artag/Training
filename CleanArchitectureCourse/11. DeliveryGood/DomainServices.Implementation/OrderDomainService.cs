using System.Linq;
using Domain.Models;
using DomainServices.Interfaces;

namespace DomainServices.Implementation
{
    public class OrderDomainService : IOrderDomainService
    {
        public decimal GetTotal(Order order, CalculateDeliveryCost calculateDeliveryCost)
        {
            var totalPrice = order.Items.Sum(x => x.Quantity * x.Product.Price);
            decimal deliveryCost = 0;
            if (totalPrice < 1000)
            {
                var totalWeight = order.Items.Sum(x => x.Product.Weight);
                deliveryCost = calculateDeliveryCost(totalWeight);
            }

            return totalPrice + deliveryCost;
        }
    }
}
