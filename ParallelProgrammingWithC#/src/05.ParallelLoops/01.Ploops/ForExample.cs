using System;
using System.Threading.Tasks;

namespace Ploops
{
    public class ForExample
    {
        public static void Execute()
        {
            Parallel.For(fromInclusive: 1, toExclusive: 11, i =>    // от 1 до 10
            {
                Console.WriteLine($"{i}^2= {i * i}");
            });

            Console.WriteLine($"The end.");
        }
    }
}
