using ContentFormatting.Middleware;
using ContentFormatting.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace ContentFormatting
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRepository, MemoryRepository>();

            services.AddMvc()
                .AddXmlDataContractSerializerFormatters() // Новый класс сериализации Xml.
                .AddMvcOptions(
                    options =>
                    {
                        options.FormatterMappings.SetMediaTypeMappingForFormat(
                            "xml", new MediaTypeHeaderValue("application/xml"));

                        options.RespectBrowserAcceptHeader = true;
                        options.ReturnHttpNotAcceptable = true;
                    }
                );

            // Старый класс сериализации Xml. Для совместимости со старыми клиентами .NET.
            // services.AddMvc().AddXmlSerializerFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseMvcWithDefaultRoute();
        }
    }
}

