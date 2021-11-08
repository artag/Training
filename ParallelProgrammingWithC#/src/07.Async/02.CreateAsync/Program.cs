using System;
using System.Threading.Tasks;

namespace CreateAsync
{
    internal class Foo
    {
        // Private ctor.
        private Foo()
        {
            Console.WriteLine($"ctor {nameof(Foo)}");
        }

        // Initialization method.
        private async Task<Foo> InitAsync()
        {
            Console.WriteLine("Initialization");
            await Task.Delay(1000);
            return this;
        }

        // Async factory method.
        public static Task<Foo> CreateAsync()
        {
            var result = new Foo();
            return result.InitAsync();
        }
    }

    internal static class Program
    {
        private static async Task Main()
        {
            await Foo.CreateAsync();

            // Display:
            // ctor Foo
            // Initialization
        }
    }
}
