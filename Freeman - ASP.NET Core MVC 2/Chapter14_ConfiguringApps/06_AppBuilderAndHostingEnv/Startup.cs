using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AppBuilderAndHostingEnv
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async (context) =>
            {
                if (env.IsDevelopment())
                {
                    await context.Response.WriteAsync("Development");
                }
                else if (env.IsStaging())
                {
                    await context.Response.WriteAsync("Staging");
                }
                else if (env.IsProduction())
                {
                    await context.Response.WriteAsync("Production");
                }
            });
        }
    }
}
