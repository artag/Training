using CloselyAndLooselyCoupled.Infrastructure;
using CloselyAndLooselyCoupled.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CloselyAndLooselyCoupled
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            TypeBroker.SetRepositoryType<AlternateRepository>();
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
