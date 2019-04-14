namespace Gcd
{
    public class CommonDivisor
    {
        public static int FindRecursiveWay(int x, int y)
        {
            if (x == y)
                return x;

            var a = x < y ? x : y;
            var b = x < y ? y : x;

            var remainder = b % a;

            return remainder == 0
                ? a
                : FindRecursiveWay(remainder, a);
        }

        public static int FindIterativeWay(int x, int y)
        {
            if (x == y)
                return x;

            var a = x < y ? x : y;
            var b = x < y ? y : x;

            var remainder = 1;
            while (remainder != 0)
            {
                remainder = a % b;

                a = b;
                b = remainder;
            }

            return a;
        }
    }
}
