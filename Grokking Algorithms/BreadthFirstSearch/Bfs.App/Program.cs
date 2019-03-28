using System;

namespace Bfs.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph();

            var person = graph.FindMangoSeller();
            Console.WriteLine($"This person is mango seller: {person}");

            person = graph.FindRobot();
            Console.WriteLine($"This person is robot: {person}");
        }
    }
}
