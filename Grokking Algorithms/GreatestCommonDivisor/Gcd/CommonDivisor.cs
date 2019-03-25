using System;

namespace Gcd
{
    public class CommonDivisor
    {
        public static int FindRecursiveWay(int x, int y)
        {
            if (x > y)
            {
                var remainder = x % y;
                if (remainder == 0)
                    return y;

                return FindRecursiveWay(remainder, y);
            }
            else
            {
                var remainder = y % x;
                if (remainder == 0)
                    return x;

                return FindRecursiveWay(remainder, x);
            }
        }

        public static int FindIterativeWay(int x, int y)
        {
            while (true)
            {
                if (x > y)
                {
                    var remainder = x % y;
                    if (remainder == 0)
                        return y;

                    x = remainder;
                }
                else
                {
                    var remainder = y % x;
                    if (remainder == 0)
                        return x;

                    y = x;
                    x = remainder;
                }
            }
        }
    }
}
