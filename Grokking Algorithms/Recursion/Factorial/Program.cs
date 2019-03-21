using System;
using InputService;

namespace Factorial
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Math.Abs(Input.IntegerNumber());

            var result = GetFactorial(input);
            Console.WriteLine($"{input}! = {result}");
        }

        private static long GetFactorial(int input)
        {
            if (input == 0 || input == 1)
                return 1;

            return input * GetFactorial(input - 1);
        }
    }
}
