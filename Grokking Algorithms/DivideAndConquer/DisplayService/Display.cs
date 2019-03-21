using System;
using System.Collections.Generic;

namespace DisplayService
{
    public class Display
    {
        public static void Numbers(IEnumerable<int> numbers)
        {
            Console.Write("Numbers: ");

            foreach (var number in numbers)
                Console.Write(number + " ");

            Console.WriteLine();
        }
    }
}
