using CreateDiffConfigMethods.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CreateDiffConfigMethods
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>(new UptimeService("Default configuration"));
            services.AddMvc();
        }

        // Перекрывает ConfigureServices в среде Development
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>(new UptimeService("Development configuration"));
            services.AddMvc();
        }

        // Перекрывает ConfigureServices в среде Production
        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>(new UptimeService("Production configuration"));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvcWithDefaultRoute();
        }

        // Перекрывает Configure в среде Development
        public void ConfigureDevelopment(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Development configure");
            });
        }

        // Перекрывает Configure в среде Production
        public void ConfigureProduction(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Production configure");
            });
        }
    }
}
