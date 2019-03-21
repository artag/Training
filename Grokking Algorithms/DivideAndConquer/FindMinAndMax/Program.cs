using System;
using System.Collections.Generic;
using System.Linq;
using DisplayService;

namespace FindMinAndMax
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new [] { 4, 1, 3, 12, 9, -2, 4};

            Display.Numbers(input);

            var max = GetExtremeValueRecursiveWay(input, getMax: true);
            Console.WriteLine($"Max number is {max}");

            var min = GetExtremeValueRecursiveWay(input, getMax: false);
            Console.WriteLine($"Min number is {min}");
        }

        private static int GetExtremeValueRecursiveWay(IEnumerable<int> numbers, bool getMax)
        {
            if (numbers.Count() == 1)
                return numbers.First();

            var result = GetExtremeValueRecursiveWay(numbers.Skip(1), getMax);

            return getMax
                ? GetMax(numbers, result)
                : GetMin(numbers, result);
        }

        private static int GetMax(IEnumerable<int> numbers, int result) =>
            numbers.First() > result
                ? numbers.First()
                : result;

        private static int GetMin(IEnumerable<int> numbers, int result) =>
            numbers.First() < result
                ? numbers.First()
                : result;
    }
}
