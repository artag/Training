using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Play.Common.MongoDB;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;

namespace Play.Inventory.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMongo()
                    .AddMongoRepository<InventoryItem>("inventoryitems");

            var jitterer = new Random();    // Для рандомизации времени попытки доступа.

            services.AddHttpClient<CatalogClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return  new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                    (message, cert, chain, errors) => true
                };
            })
            // (1) AddTransientHttpErrorPolicy добавляется до AddPolicyHandler
            // (2) Specifies the type of exception that this policy can handle.
            // (3) Количество повторов соединения
            // (4) Время между повторами. Каждый раз увеличивается в 2 раза.
            // (5) Небольшой случайный разброс по времени нужен для сглаживания пиковой нагрузки на
            //     сервис, если сразу несколько клиентов будут делать повторные запросы.
            // (6) Делегат, срабатывающий при повторе. Логгер здесь достается через serviceProvider.
            //     В production code так делать НЕ НАДО, только для учебного примера.
            .AddTransientHttpErrorPolicy(builder =>                                     // (1)
                builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(               // (2)
                    retryCount: 5,                                                      // (3)
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +   // (4)
                                    TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),  // (5)
                    onRetry: (outcome, timespan, retryAttempt) => {                     // (6)
                        var serviceProvider = services.BuildServiceProvider();
                        serviceProvider.GetService<ILogger<CatalogClient>>()?
                            .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, than making retry {retryAttempt}");
                    }
                ))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Inventory.Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Inventory.Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
