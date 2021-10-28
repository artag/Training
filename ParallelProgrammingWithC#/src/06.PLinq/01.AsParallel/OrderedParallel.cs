using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsParallel
{
    public static class OrderedParallel
    {
        public static void Execute()
        {
            const int count = 50;
            var items = Enumerable.Range(1, count).ToArray();

            var cubes = items.AsParallel().AsOrdered().Select(x => x * x * x);

            foreach (var result in cubes)
                Console.Write($"{result}\t");

            Console.WriteLine();
        }
    }
}
