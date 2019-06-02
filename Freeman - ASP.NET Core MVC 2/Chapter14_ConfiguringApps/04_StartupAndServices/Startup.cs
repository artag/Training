using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StartupAndServices.Infrastructure;

namespace StartupAndServices
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>();

            // Включение MVC (для демонстрации).
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Включение MVC (для демонстрации).
            app.UseMvcWithDefaultRoute();
        }
    }
}
