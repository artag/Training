using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(MessageDto message);
    }
}
