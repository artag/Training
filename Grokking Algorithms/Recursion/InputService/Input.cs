using System;

namespace InputService
{
    public class Input
    {
        public static int IntegerNumber()
        {
            var result = 0;
            while (result == 0)
                result = TryToGetNumber();

            return result;
        }

        private static int TryToGetNumber()
        {
            Console.Clear();
            Console.Write("Enter integer number: ");

            int result;
            var input = Console.ReadLine();
            int.TryParse(input, out result);

            return result;
        }
    }
}
