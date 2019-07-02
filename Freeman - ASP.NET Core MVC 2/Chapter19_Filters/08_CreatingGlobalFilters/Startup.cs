using CreatingGlobalFilters.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CreatingGlobalFilters
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DiagnosticsFilter>();
            services.AddScoped<ViewResultDiagnostics>();
            services.AddScoped<IFilterDiagnostics, DefaultFilterDiagnostics>();
            services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.AddService(typeof(ViewResultDiagnostics));
                options.Filters.AddService(typeof(DiagnosticsFilter));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseMvcWithDefaultRoute();
        }
    }
}
