using ApplicationServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationServices
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            return services;
        }
    }
}
