using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public class Display
    {
        public static void MinimalCostPath(Stack<string> itemsToDisplay)
        {
            while (itemsToDisplay.TryPop(out var node))
            {
                Console.Write(node);

                if (itemsToDisplay.Any())
                    Console.Write(" > ");
            }

            Console.WriteLine();
        }

        public static void MinimalCost(int minimalCost)
        {
            Console.WriteLine($"Minimal cost: {minimalCost}");
        }
    }
}
