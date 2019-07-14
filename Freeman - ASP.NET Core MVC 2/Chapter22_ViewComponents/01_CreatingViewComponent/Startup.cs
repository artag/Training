using CreatingViewComponent.Middleware;
using CreatingViewComponent.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CreatingViewComponent
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, MemoryProductRepository>();
            services.AddSingleton<ICityRepository, MemoryCityRepository>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseMvcWithDefaultRoute();
        }
    }
}
