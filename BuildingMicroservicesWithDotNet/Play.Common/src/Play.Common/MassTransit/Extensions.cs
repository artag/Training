using System;
using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                // Регистрация consumers. Adds all consumers in the specified assemblies.
                configure.AddConsumers(Assembly.GetEntryAssembly());

                // Задание транспорта, который будет использоваться (RabbitMQ)
                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, includeNamespace: false));
                    // Добавление повтора сообщений если consumer не смог его обработать
                    // по какой-либо причине.
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(retryCount: 3, interval: TimeSpan.FromSeconds(5));
                    });
                });
            });

            // To start MassTransit service. This service starts RabbitMQ bus.
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
