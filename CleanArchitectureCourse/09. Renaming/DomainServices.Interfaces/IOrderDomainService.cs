using Domain.Models;

namespace DomainServices.Interfaces
{
    public interface IOrderDomainService
    {
        decimal GetTotal(Order order);
    }
}
