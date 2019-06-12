using InlineConstrainingRoutes.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace InlineConstrainingRoutes
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(
                options => options.ConstraintMap.Add("weekday", typeof(WeekDayConstraint)));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "WeekDayConstraint",
                    template: "{controller=WeekDay}/{action=Index}/{id:weekday?}");

                routes.MapRoute(
                    name: "UnionConstraint",
                    template: "{controller=Union}/{action=Index}/{id:alpha:minlength(6)?}");

                routes.MapRoute(
                    name: "IntRangeConstraint",
                    template: "{controller=Admin}/{action=CustomVariable}/{id:range(10,20)?}");

                routes.MapRoute(
                    name: "RegexConstraint1",
                    template: "{controller:regex(^H.*)=Home}/{action:regex(^Index$|^About$)=Index}/{id?}");

                routes.MapRoute(
                    name: "RegexConstraint2",
                    template: "{controller:regex(^H.*)=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "IntConstraint",
                    template: "{controller=Home}/{action=Index}/{id:int?}");
            });
        }
    }
}
