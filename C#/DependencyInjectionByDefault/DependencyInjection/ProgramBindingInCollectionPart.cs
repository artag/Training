using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    partial class Program
    {
        private static void BindingInCollectionTestsRun(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IService), p => new ServiceOne(), ServiceLifetime.Singleton));
            services.Add(new ServiceDescriptor(typeof(IService), p => new ServiceTwo(), ServiceLifetime.Singleton));

            var provider = services.BuildServiceProvider();

            var collectionServices = provider.GetService<IEnumerable<IService>>().ToList();
            foreach (var s in collectionServices)
            {
                s.DoIt();
            }

            ConsoleEx.WaitingForKeyPressing();
        }
    }
}
