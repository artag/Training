using System;

namespace Flyweight
{
    public class Program
    {
        static void Main()
        {
            var treeTypeFactory = new TreeTypeFactory();
            var forest = new Forest(treeTypeFactory, "grass");
            forest.PlantTree(1, 1, "Green Big", "green");
            forest.PlantTree(2, 2, "Blue Small", "blue");
            forest.PlantTree(3, 3, "Green Big", "green");
            forest.PlantTree(4, 4, "Blue Small", "blue");
            Console.WriteLine();

            forest.Draw();
        }
    }
}
