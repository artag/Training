using System;
using InputService;

namespace Countdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Input.IntegerNumber();

            if (input > 0)
                Countdown(input);
            else
                Countup(input);
        }

        static void Countdown(int initialNumber)
        {
            if (IsBreakCase(initialNumber))
                return;

            Console.WriteLine(initialNumber);
            Countdown(initialNumber - 1);
        }

        static void Countup(int initialNumber)
        {
            if (IsBreakCase(initialNumber))
                return;

            Console.WriteLine(initialNumber);
            Countup(initialNumber + 1);
        }

        private static bool IsBreakCase(int initialNumber)
        {
            if (initialNumber != 0)
                return false;

            Console.WriteLine("Fire!");
            return true;
        }
    }
}
