using System.Diagnostics.CodeAnalysis;

namespace Fibonacci
{
    public static class Fibonacci
    {
        public static long GetNumberRecursive(int index)
        {
            if (index == 0)
                return 0;

            if (index == 1)
                return 1;

            return GetNumberRecursive(index - 2) + GetNumberRecursive(index - 1);
        }

        public static long GetNumberIterative(int index)
        {
            long first = 0;
            if (index == 0)
                return 0;

            long second = 1;
            if (index == 1)
                return second;

            long current = 0;
            for (var i = 2; i <= index; i++)
            {
                current = first + second;
                first = second;
                second = current;
            }

            return current;
        }
    }
}
