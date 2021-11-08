using System.Threading.Tasks;
using System;
using Nito.AsyncEx;

namespace AsyncLazyInit
{
    public class Stuff
    {
        private readonly Lazy<Task<int>> LazyValue1 =
            new Lazy<Task<int>>(async () =>
            {
                Console.WriteLine($"Enter {nameof(LazyValue1)}");

                await Task.Delay(1000).ConfigureAwait(false);
                const int result = 1;

                Console.WriteLine($"Exit {nameof(LazyValue1)}");
                return result;
            });

        private readonly Lazy<Task<int>> LazyValue2 =
            new Lazy<Task<int>>(Task.Run(async() =>
            {
                Console.WriteLine($"Enter {nameof(LazyValue2)}");

                await Task.Delay(1000);
                const int result = 2;

                Console.WriteLine($"Exit {nameof(LazyValue2)}");
                return result;
            }));

        // Nito.AsyncEx
        private readonly AsyncLazy<int> LazyValue3 =
            new AsyncLazy<int>(async() =>
            {
                Console.WriteLine($"Enter {nameof(LazyValue3)}");

                await Task.Delay(1000);
                const int result = 3;

                Console.WriteLine($"Exit {nameof(LazyValue3)}");
                return result;
            });

        public async Task UseValues()
        {
            Console.WriteLine($"Use value 1 = {await LazyValue1.Value}");
            Console.WriteLine($"Use value 2 = {await LazyValue2.Value}");
            Console.WriteLine($"Use value 3 = {await LazyValue3}");
        }
    }

    public static class Program
    {
        public static async Task Main()
        {
            var stuff = new Stuff();
            await stuff.UseValues();

            // Display:
            // Enter LazyValue2
            // Enter LazyValue1
            // Exit LazyValue1
            // Exit LazyValue2
            // Use value 1 = 1
            // Use value 2 = 2
            // Enter LazyValue3
            // Exit LazyValue3
            // Use value 3 = 3
        }
    }
}
