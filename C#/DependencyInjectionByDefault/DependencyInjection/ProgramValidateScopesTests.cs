using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    partial class Program
    {
        private static void ValidateScopesTestsRun(ServiceCollection services)
        {
            services.AddScoped(p => "123");

            var provider = services.BuildServiceProvider();
            var s = provider.GetService<string>();
            Console.WriteLine(s);       // 123

            // Если установить параметр у метода-расширения BuildServiceProvider
            // validateScopes в true, то попытка достать сервисы, добавленные в
            // ServiceLifetime.Scoped, в корневом ServiceProvider приведет к исключению
            provider = services.BuildServiceProvider(validateScopes: true);

            try
            {
                s = provider.GetService<string>();
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc);
            }

            ConsoleEx.WaitingForKeyPressing();
        }
    }
}
