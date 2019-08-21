using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    partial class Program
    {
        private static void BindingGenericTestsRun(IServiceCollection services)
        {
            services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));

            var provider = services.BuildServiceProvider();

            var strObject = provider.GetService<IGenericService<string>>();
            var intObject = provider.GetService<IGenericService<int>>();
            var dtObject = provider.GetService<IGenericService<DateTime>>();

            Console.WriteLine($"string: {strObject.Create()}");
            Console.WriteLine($"int: {intObject.Create()}");
            Console.WriteLine($"DateTime: {dtObject.Create()}");

            ConsoleEx.WaitingForKeyPressing();
        }
    }
}
