using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Delivery.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Mobile.UseCases.Order.BackgroundJobs;

public class UpdateDeliveryStatusJob : IJob
{
    private readonly IDbContext _dbContext;
    private readonly IDeliveryService _deliveryService;

    public UpdateDeliveryStatusJob(IDbContext dbContext, IDeliveryService deliveryService)
    {
        _dbContext = dbContext;
        _deliveryService = deliveryService;
    }

    public async Task ExecuteAsync()
    {
        // Получение заказов из БД.
        var orders = await _dbContext.Orders
            .Where(x => x.Status == OrderStatus.Created)
            .ToListAsync();

        // Получение информации о статусе заказов.
        var items = orders
            .Select(x => new { Order = x, Task = _deliveryService.IsDeliveredAsync(x.Id) })
            .ToList();

        await Task.WhenAll(items.Select(x => x.Task));

        // Бизнес операция. Обновление статуса заказа.
        foreach (var item in items)
        {
            var delivered = await item.Task;
            if (delivered)
            {
                item.Order.Status = OrderStatus.Delivered;
            }
        }

        // Сохранение заказов в БД.
        await _dbContext.SaveChangesAsync();
    }
}
