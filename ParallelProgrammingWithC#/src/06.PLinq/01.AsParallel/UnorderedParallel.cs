using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsParallel
{
    public static class UnorderedParallel
    {
        public static void Execute()
        {
            const int count = 50;

            var items = Enumerable.Range(1, count).ToArray();
            var results = new int[count];

            items.AsParallel().ForAll(x =>
            {
                var newValue = x * x * x;
                Console.Write($"{newValue} (Task {Task.CurrentId})\t");
                results[x - 1] = newValue;
            });

            Console.WriteLine();
            Console.WriteLine();

            foreach (var result in results)
                Console.WriteLine(result);
        }
    }
}
