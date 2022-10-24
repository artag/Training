using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Application;

public interface IOrderService
{
    Task<OrderDto> GetByIdAsync(int id);
}
