using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicProgramming
{
    public class GreatestCommonDivisor
    {
        public static double Find(IEnumerable<double> numbers)
        {
            if (!numbers.Any())
                throw new ArgumentOutOfRangeException(
                    nameof(numbers), "Count of items in the input enumerable must be more than 0.");

            if (numbers.Count() == 1)
                return numbers.First();

            return FindGdcInEnumeration(numbers);
        }

        public static double Find(double a, double b)
        {
            if (Math.Abs(a - b) < double.Epsilon)
                return a;

            var x = a < b ? a : b;
            var y = a < b ? b : a;

            var remainder = 1.0;
            while (Math.Abs(remainder) > double.Epsilon)
            {
                remainder = Math.Round(x % y, 2);

                x = y;
                y = remainder;
            }

            return x;
        }

        private static double FindGdcInEnumeration(IEnumerable<double> numbers)
        {
            var items = numbers.ToList();

            var first = items.First();
            var last = items.Last();
            items.Remove(first);
            items.Remove(last);

            first = Find(first, last);
            while (items.Any())
            {
                last = items.Last();
                items.Remove(last);
                first = Find(first, last);
            }

            return first;
        }
    }
}
