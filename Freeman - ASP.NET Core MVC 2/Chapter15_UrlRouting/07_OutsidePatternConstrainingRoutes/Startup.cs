using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;
using OutsidePatternConstrainingRoutes.Infrastructure;

namespace OutsidePatternConstrainingRoutes
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "WeekDayConstraint",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "WeekDay", action = "Index" },
                    constraints: new { id = new WeekDayConstraint() });

                routes.MapRoute(
                    name: "UnionConstraint",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Union", action = "Index" },
                    constraints: new
                    {
                        id = new CompositeRouteConstraint(
                            new IRouteConstraint[]
                            {
                                new AlphaRouteConstraint(),
                                new MinLengthRouteConstraint(6)
                            })
                    });

                routes.MapRoute(
                    name: "IntConstraint",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller="Home", action = "Index" },
                    constraints: new { id = new IntRouteConstraint() });
            });
        }
    }
}
