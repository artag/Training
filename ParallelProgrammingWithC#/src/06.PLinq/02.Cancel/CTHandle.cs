using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cancel
{
    internal static class CTHandle
    {
        public static void Execute()
        {
            var items = ParallelEnumerable.Range(1, 20);

            var cts = new CancellationTokenSource();
            var results = items.WithCancellation(cts.Token).Select(i =>
            {
                var result = Math.Log10(i);
                Console.WriteLine($"i = {i}, tid = {Task.CurrentId}");
                return result;
            });

            try
            {
                foreach (var r in results)
                {
                    if (r > 1)
                        cts.Cancel();

                    Console.WriteLine($"result = {r}");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Canceled");
            }
        }
    }
}
