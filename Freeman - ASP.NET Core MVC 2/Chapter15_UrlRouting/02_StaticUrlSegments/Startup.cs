using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace StaticUrlSegments
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

            app.UseMvc(routes =>
            {
                // Воспринимает сегмент:
                //    /Shop/OldAction -> /Home/Index
                routes.MapRoute(
                    name: "ShopSchema2",
                    template: "Shop/OldAction",
                    defaults: new { controller = "Home", action = "Index" });

                // Воспринимает сегмент:
                //    /Shop/Index -> /Home/Index
                routes.MapRoute(
                    name: "ShopSchema",
                    template: "Shop/{action}",
                    defaults: new { controller = "Home" });

                // Воспринимает сегменты вида:
                //    /XHome/Index     -> /Home/Index
                //    /XAdmin/Index    -> /Admin/Index
                //    /XCustomer/Index -> /Customer/Index
                //    /XCustomer/List  -> /Customer/List
                routes.MapRoute(
                    name: "",
                    template: "X{controller}/{action}");

                // Воспринимает сегменты вида:
                //    /              -> /Home/Index
                //    /Home          -> /Home/Index
                //    /Admin         -> /Admin/Index
                //    /Customer      -> /Customer/Index
                //    /Customer/List -> /Customer/List
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "",
                    template: "Public/{controller=Home}/{action=Index}");
            });
        }
    }
}
