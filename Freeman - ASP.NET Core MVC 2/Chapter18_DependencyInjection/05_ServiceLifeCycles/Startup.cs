using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServiceLifeCycles.Models;

namespace ServiceLifeCycles
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITransientDI, TransientDIImpl>();
            services.AddTransient<TransientDI>();

            services.AddScoped<IScopedDI, ScopedDIImpl>();
            services.AddScoped<ScopedDI>();

            services.AddSingleton<ISingletonDI, SingletonDIImpl>();
            services.AddSingleton<SingletonDI>();

            services.AddTransient<ITransientFactoryDI>(provider => new TransientFactoryDIImpl());

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseMvcWithDefaultRoute();
        }
    }
}
