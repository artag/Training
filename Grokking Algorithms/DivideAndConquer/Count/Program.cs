using System;
using System.Collections.Generic;
using System.Linq;
using DisplayService;

namespace Count
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new [] { 4, 1, 3, 12, 9, -2, 4};

            Display.Numbers(input);

            var count = GetCountRecursiveWay(input);
            Console.WriteLine($"The number of elements is {count}");
        }

        private static int GetCountRecursiveWay(IEnumerable<int> numbers)
        {
            if (!numbers.Any())
                return 0;

            return GetCountRecursiveWay(numbers.Skip(1)) + 1;
        }
    }
}
