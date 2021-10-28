using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cancel
{
    internal static class ExceptionHandle
    {
        public static void Execute()
        {
            var items = ParallelEnumerable.Range(1, 20);
            var results = items.Select(i =>
            {
                var result = Math.Log10(i);

                // Для демонстрации обработки исключения.
                if (result > 1) throw new InvalidOperationException();

                Console.WriteLine($"i = {i}, tid = {Task.CurrentId}");
                return result;
            });

            try
            {
                foreach (var r in results)
                    Console.WriteLine($"result = {r}");
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                    return true;
                });
            }
        }
    }
}
