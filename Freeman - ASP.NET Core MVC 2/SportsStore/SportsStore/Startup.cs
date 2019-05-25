using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using WorkingWithVisualStudioTests.Middleware;

namespace SportsStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlServerOptions = Configuration["Data:SportsStoreProducts:ConnectionString"];
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(sqlServerOptions));
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(serviceProvider => SessionCart.GetCart(serviceProvider));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseSession();
            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        name: null,
                        template: "{category}/Page{productPage:int}",
                        defaults: new { controller = "Product", action = "List"});

                    routes.MapRoute(
                        name: null,
                        template: "Page{productPage:int}",
                        defaults: new { controller = "Product", action = "List", productPage = 1});

                    routes.MapRoute(
                        name: null,
                        template: "{category}",
                        defaults: new { controller = "Product", action = "List", productPage = 1 });

                    routes.MapRoute(
                        name: null,
                        template: "",
                        defaults: new { controller="Product", action="List", productPage = 1 });

                    routes.MapRoute(
                        name: null,
                        template: "{controller}/{action}/{id?}");
                });

            SeedData.EnsurePopulated(app);
        }
    }
}
