using Delivery.Interfaces;

namespace Delivery.DeliveryCompany
{
    public class DeliveryService : IDeliveryService
    {
        public decimal CalculateDeliveryCost(float weight)
        {
            return (decimal)weight * 10;
        }
    }
}
