using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicProgramming.App
{
    public static class Display
    {
        public static void Items(IEnumerable<Item> items, int maxWeight)
        {
            Console.WriteLine("Items list: ");

            var i = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"{i++}. {item}");
            }

            Console.WriteLine();
            Console.WriteLine($"Max weight: {maxWeight}");
            Console.WriteLine();
        }

        public static void ItemsToGet(IEnumerable<Item> items, int maxWeight)
        {
            Console.WriteLine("Items to get: ");

            var i = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"{i++}. {item}");
            }

            Console.WriteLine("---");

            var cost = $"Total cost: {items.Select(item => item.Cost).Sum()}";
            Console.WriteLine(cost);

            var weight = $"Total weight: {items.Select(item => item.Weight).Sum()} from {maxWeight}";
            Console.WriteLine(weight);

            Console.WriteLine("==========");
            Console.WriteLine();
        }

        public static void PauseAndClear()
        {
            Pause();
            Clear();
        }

        public static void Pause()
        {
            Console.WriteLine("\nPress \"Enter\" to continue...");
            Console.ReadLine();
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
