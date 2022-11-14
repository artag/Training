using Domain.Models;
using DomainServices.Interfaces;

namespace DomainServices.Implementation;

public class OrderDomainService : IOrderDomainService
{
    public decimal GetTotal(Order order, CalculateDeliveryCost deliveryCostCalculator)
    {
        var totalWeight = order.Items.Sum(x => x.Product.Weight);
        var deliveryCost = deliveryCostCalculator(totalWeight);
        var total = order.Items.Sum(x => x.Quantity * x.Product.Price) + deliveryCost;
        return total;
    }
}
