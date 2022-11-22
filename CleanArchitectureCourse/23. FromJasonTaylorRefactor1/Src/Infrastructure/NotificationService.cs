using Northwind.Application.Common.Interfaces;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Northwind.Infrastructure
{
    public class NotificationService : INotificationService
    {
        public Task SendAsync(MessageDto message)
        {
            return Task.CompletedTask;
        }
    }
}
