using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Infrastructure;

namespace Middleware
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Демонстрация Middleware для редактирования запросов.
            // Плюс генерация содержимого через передачу параметров через HttpContext.Items.
            app.UseMiddleware<BrowserTypeMiddleware>();
            app.UseMiddleware<ContentMiddlewareForItems>();


            // Демонстрация Middleware для редактирования ответов.
            app.UseMiddleware<ErrorMiddleware>();
            app.UseMiddleware<ShortCircuitMiddlewareSendError>();

            // Демонстрация Middleware для обхода.
            app.UseMiddleware<ShortCircuitMiddleware>();

            // Демонстрация Middleware для генерации содержимого.
            app.UseMiddleware<ContentMiddleware>();
            app.UseMiddleware<ContentMiddlewareWithService>();

            app.Run(async (context) =>
            {
                var message = "Type one url from the next ones:\n" +
                              "/content - Content-Generating Middleware.\n" +
                              "/timer   - Content-Generating Middleware using service.\n" +
                              "/browser - Request-Editing Middleware.\n" +
                              "/error   - Response-Editing Middleware.";

                await context.Response.WriteAsync(message);
            });
        }
    }
}
