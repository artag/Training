using Microsoft.Extensions.DependencyInjection;
using Notification.Interfaces;

namespace Notification
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotification(this IServiceCollection services)
        {
            services.AddTransient<INotificationService, NotificationService>();
            return services;
        }
    }
}
