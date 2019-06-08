using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DefaultValues
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            // Маршрутизация
            app.UseMvc(routes =>
                       {
                           // Простой маршрут
                           // ---
                           // Откликается на адреса:
                           // /Admin/Index
                           // /Customer/Index
                           // /Customer/List
                           // /Home/Index
                           routes.MapRoute(
                               name: "base",
                               template: "{controller}/{action}");

                           // Маршрут со стандартными значениями
                           // ---
                           // Откликается на адреса:
                           // /Admin    - перейдет на (/Admin/Index)
                           // /Customer - перейдет на (/Customer/Index)
                           // /Home     - перейдет на (/Home/Index)
                           routes.MapRoute(
                               name: "default_with_value",
                               template: "{controller}/{action}",
                               defaults: new { action = "Index" });

                           // Маршрут со встраиваемыми стандартными значениями
                           // ---
                           // Откликается на адреса:
                           // /Admin    - перейдет на (/Admin/Index)
                           // /Customer - перейдет на (/Customer/Index)
                           // /Home     - перейдет на (/Home/Index)
                           routes.MapRoute(
                               name: "default_with_embedded_value",
                               template: "{controller}/{action=Index}");

                           // Маршрут со встраиваемыми стандартными значениями (для всех сегментов)
                           // ---
                           // / - перейдет на (/Home/Index)
                           routes.MapRoute(
                               name: "default",
                               template: "{controller=Home}/{action=Index}");
                       });
        }
    }
}
