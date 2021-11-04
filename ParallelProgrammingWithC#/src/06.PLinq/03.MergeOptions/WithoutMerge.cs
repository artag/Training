using System.Linq;
using System;

namespace MergeOptions
{
    internal static class WithoutMerge
    {
        public static void Execute()
        {
            var numbers = Enumerable.Range(1, 20).ToArray();
            var results = numbers.AsParallel()
                .Select(x =>
                {
                    var result = Math.Log10(x);
                    Console.WriteLine($"Produced {result}");
                    return result;
                });

            foreach (var result in results)
            {
                Console.WriteLine($"Consumed {result}");
            }
        }
    }
}
