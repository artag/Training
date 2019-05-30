using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            var sportsStoreDbOptions = Configuration["Data:SportsStoreProducts:ConnectionString"];
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(sportsStoreDbOptions));

            var identityDbOptions = Configuration["Data:SportsStoreIdentity:ConnectionString"];
            services.AddDbContext<AppIdentityDbContext>(
                options => options.UseSqlServer(identityDbOptions));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(serviceProvider => SessionCart.GetCart(serviceProvider));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseSession();
            app.UseAuthentication();
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

            // Начальное заполнение данными в Production будет производиться другим способом.
            //SeedData.EnsurePopulated(app);
            //IdentitySeedData.EnsurePopulated(app);
        }
    }
}
