using System;
using System.Linq;

namespace CustomAggregation
{
    internal static class Program
    {
        private static void Main()
        {
            // Sequentional (последовательное выполнение вычислений)

            // Частные случаи операции Aggregate
            var sum = Enumerable.Range(1, 1000).Sum();          // = 500500
            var average = Enumerable.Range(1, 1000).Average();

            Console.WriteLine($"Sum = {sum}");
            Console.WriteLine($"Average = {average}");

            var sum1 = Enumerable.Range(1, 1000)
                .Aggregate(seed: 0, (i, acc) => i + acc);

            Console.WriteLine($"Sequential sum = {sum1}");

            // Parallel (параллельное выполнение вычислений)
            var sum2 = ParallelEnumerable.Range(1, 1000)
                .Aggregate(
                    seed: 0,
                    (partialSum, i) => partialSum += i,
                    (total, subtotal) => total += subtotal,
                    i => i
                );

            Console.WriteLine($"Parallel sum = {sum2}");
        }
    }
}
