using System.Threading.Tasks;

namespace Delivery.Interfaces
{
    public interface IDeliveryService
    {
        decimal CalculateDeliveryCost(float weight);
        Task<bool> IsDeliveredAsync(int orderId);
    }
}
