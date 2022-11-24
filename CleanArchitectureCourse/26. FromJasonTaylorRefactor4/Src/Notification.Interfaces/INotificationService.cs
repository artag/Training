using System.Threading.Tasks;

namespace Notification.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(MessageDto message);
    }
}
