using DomainServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderDomainService, OrderDomainService>();
            return services;
        }
    }
}
