using System.Threading.Tasks;
using Delivery.Interfaces;

namespace Delivery.DeliveryCompany
{
    public class DeliveryService : IDeliveryService
    {
        public decimal CalculateDeliveryCost(float weight)
        {
            return (decimal)weight * 10;
        }

        public Task<bool> IsDeliveredAsync(int orderId)
        {
            return Task.FromResult(true);
        }
    }
}
