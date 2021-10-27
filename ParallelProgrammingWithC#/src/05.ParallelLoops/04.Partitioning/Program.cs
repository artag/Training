using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Partitioning
{
    public class Program
    {
        /// <summary>
        /// Возводит в квадрат. Медленная операция.
        /// </summary>
        [Benchmark]
        public void SquareEachValue()
        {
            const int count = 100000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];
            Parallel.ForEach(values, x => results[x] = (int)Math.Pow(x, 2));
        }

        /// <summary>
        /// Возводит в квадрат. Быстрая операция.
        /// </summary>
        [Benchmark]
        public void SquareEachValueChunked()
        {
            const int count = 100000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];

            var part = Partitioner.Create(fromInclusive: 0, toExclusive: count, rangeSize: 10000);
            Parallel.ForEach(source: part, body: range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    results[i] = (int)Math.Pow(i, 2);
                }
            });
        }

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
            Console.WriteLine(summary);
        }
    }
}
