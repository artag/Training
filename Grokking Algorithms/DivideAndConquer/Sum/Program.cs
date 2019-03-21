using System;
using System.Collections.Generic;
using System.Linq;
using DisplayService;

namespace Sum
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new [] { 4, 1, 3, 2 };

            Display.Numbers(input);

            var sum = SumIterationWay(input);
            Console.WriteLine("\nFind sum in iteration way:");
            Console.WriteLine($"Sum = {sum}");

            sum = SumRecursiveWay(input);
            Console.WriteLine("\nFind sum in recursive way:");
            Console.WriteLine($"Sum = {sum}");
        }

        private static int SumIterationWay(IEnumerable<int> input) => input.Sum();

        private static int SumRecursiveWay(IEnumerable<int> input)
        {
            if (input.Count() == 1)
                return input.First();

            return input.First() + SumRecursiveWay(input.Skip(1));
        }
    }
}
