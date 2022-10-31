using System.Linq;
using Delivery.Interfaces;
using Domain.Models;
using DomainServices.Interfaces;

namespace DomainServices.Implementation
{
    public class OrderDomainService : IOrderDomainService
    {
        private readonly IDeliveryService _deliveryService;

        public OrderDomainService(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        public decimal GetTotal(Order order)
        {
            var totalPrice = order.Items.Sum(x => x.Quantity * x.Product.Price);

            decimal deliveryCost = 0;
            if (totalPrice < 1000)
            {
                var totalWeight = order.Items.Sum(x => x.Product.Weight);
                deliveryCost = _deliveryService.CalculateDeliveryCost(totalWeight);
            }

            return totalPrice + deliveryCost;
        }
    }
}
