using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    partial class Program
    {
        private static void DisposeTestsRun(IServiceCollection services)
        {
            var disposeTests = new DisposeTests(Run);

            Console.WriteLine("Test Transient");
            var transient = SetTransient(services);
            disposeTests.Run(transient);

            Console.WriteLine("Test Scoped");
            var scoped = SetScoped(services);
            disposeTests.Run(scoped);

            Console.WriteLine("Test Singleton");
            var singleton = SetSingleton(services);
            disposeTests.Run(singleton);
        }

        private static ServiceProvider SetTransient(IServiceCollection services)
        {
            services.AddTransient<DisposeMe, DisposeMe>();
            services.AddTransient<IDisposeMeTo, DisposeMeTo>();

            var provider = services.BuildServiceProvider();
            return provider;
        }

        private static IServiceProvider SetScoped(IServiceCollection services)
        {
            services.AddScoped<DisposeMe, DisposeMe>();
            services.AddScoped<IDisposeMeTo, DisposeMeTo>();

            var provider = services.BuildServiceProvider();
            return provider;
        }

        private static IServiceProvider SetSingleton(IServiceCollection services)
        {
            services.AddSingleton<DisposeMe, DisposeMe>();
            services.AddSingleton<IDisposeMeTo, DisposeMeTo>();

            var provider = services.BuildServiceProvider();
            return provider;
        }

        public static void Run(IServiceProvider provider)
        {
            Console.WriteLine("\n--- Run ---");
            provider.GetService<DisposeMe>().DoIt();
            Console.WriteLine();
        }
    }
}
