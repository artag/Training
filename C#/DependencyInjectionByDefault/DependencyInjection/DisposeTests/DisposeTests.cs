using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    class DisposeTests
    {
        private readonly Action<IServiceProvider> _runMethod;

        public DisposeTests(Action<IServiceProvider> runMethod)
        {
            _runMethod = runMethod;
        }

        public void Run(IServiceProvider provider)
        {
            Execute(provider);
            ConsoleEx.WaitingForKeyPressing();

            ExecuteWithSingleScope(provider);
            ConsoleEx.WaitingForKeyPressing();

            ExecuteWithDoubleScopes(provider);
            ConsoleEx.WaitingForKeyPressing();

            ExecuteWithDoubleScopes2(provider);
            ConsoleEx.WaitingForKeyPressing();
        }

        private void Execute(IServiceProvider provider)
        {
            _runMethod(provider);
        }

        private void ExecuteWithSingleScope(IServiceProvider provider)
        {
            Console.WriteLine("--- Enter outer scope ---");
            using (var outerScope = provider.CreateScope())
            {
                _runMethod(outerScope.ServiceProvider);
                Console.WriteLine("--- Close outer scope ---");
            }
        }

        private void ExecuteWithDoubleScopes(IServiceProvider provider)
        {
            Console.WriteLine("--- Enter outer scope ---");
            using (var outerScope = provider.CreateScope())
            {
                _runMethod(outerScope.ServiceProvider);

                Console.WriteLine("--- Enter inner scope ---");
                using (var innerScope = outerScope.ServiceProvider.CreateScope())
                {
                    _runMethod(innerScope.ServiceProvider);
                    Console.WriteLine("--- Close inner scope ---");
                }

                Console.WriteLine("--- Close outer scope ---");
            }
        }

        private void ExecuteWithDoubleScopes2(IServiceProvider provider)
        {
            Console.WriteLine("--- Enter outer scope ---");
            using (var outerScope = provider.CreateScope())
            {
                _runMethod(outerScope.ServiceProvider);

                Console.WriteLine("--- Enter inner scope ---");
                using (var innerScope = outerScope.ServiceProvider.CreateScope())
                {
                    _runMethod(innerScope.ServiceProvider);
                    Console.WriteLine("--- Close inner scope ---");
                }

                _runMethod(outerScope.ServiceProvider);
                Console.WriteLine("--- Close outer scope ---");
            }
        }
    }
}
