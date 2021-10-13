using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CStack
{
    class Program
    {
        static void Main(string[] args)
        {
            var stack = new ConcurrentStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);

            int result;
            if (stack.TryPeek(out result))
                Console.WriteLine($"{result} is on top");

            if (stack.TryPop(out result))
                Console.WriteLine($"Popped {result}");

            var items = new int[5];
            items[3] = 77;
            items[4] = 666; 
            if (stack.TryPopRange(items, startIndex: 0, count: 5) > 0)
            {
                var text = string.Join(", ", items.Select(i => i.ToString()));
                Console.WriteLine($"Popped these items: {text}");
            }
        }
    }
}
