using System;

namespace DijkstraAlgorithm.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var algo = new Algorithm();

            Test1(algo);
            Test2(algo);
            Test3(algo);
            Test4(algo);
        }

        static void Test1(Algorithm algo)
        {
            var graph = new Graph();
            graph.AddNode("start");
            graph.AddNode("a");
            graph.AddNode("b");
            graph.AddNode("fin");

            graph.AddEdge("start", "a", 6);
            graph.AddEdge("start", "b", 2);
            graph.AddEdge("a", "fin", 1);
            graph.AddEdge("b", "a", 3);
            graph.AddEdge("b", "fin", 5);

            Console.WriteLine("Test1");
            algo.DisplayMinimalCost(graph, "start", "fin");
            Console.WriteLine();
        }

        static void Test2(Algorithm algo)
        {
            var graph = new Graph();
            graph.AddNode("book");
            graph.AddNode("vinyl");
            graph.AddNode("poster");
            graph.AddNode("bass guitar");
            graph.AddNode("drum");
            graph.AddNode("piano");

            graph.AddEdge("book", "vinyl", 5);
            graph.AddEdge("book", "poster", 0);
            graph.AddEdge("vinyl", "bass guitar", 15);
            graph.AddEdge("vinyl", "drum", 20);
            graph.AddEdge("poster", "bass guitar", 30);
            graph.AddEdge("poster", "drum", 35);
            graph.AddEdge("bass guitar", "piano", 20);
            graph.AddEdge("drum", "piano", 10);

            Console.WriteLine("Test2");
            algo.DisplayMinimalCost(graph, "book", "piano");
            Console.WriteLine();
        }

        static void Test3(Algorithm algo)
        {
            var graph = new Graph();
            graph.AddNode("start");
            graph.AddNode("end");
            graph.AddNode("a");
            graph.AddNode("b");
            graph.AddNode("c");
            graph.AddNode("d");

            graph.AddEdge("start", "a", 5);
            graph.AddEdge("start", "b", 2);
            graph.AddEdge("a", "c", 4);
            graph.AddEdge("a", "d", 2);
            graph.AddEdge("b", "a", 8);
            graph.AddEdge("b", "d", 7);
            graph.AddEdge("c", "end", 3);
            graph.AddEdge("c", "d", 6);
            graph.AddEdge("d", "end", 1);

            Console.WriteLine("Test3");
            algo.DisplayMinimalCost(graph, "start", "end");
            Console.WriteLine();
        }

        static void Test4(Algorithm algo)
        {
            var graph = new Graph();
            graph.AddNode("start");
            graph.AddNode("end");
            graph.AddNode("a");
            graph.AddNode("b");
            graph.AddNode("c");

            graph.AddEdge("start", "a", 10);
            graph.AddEdge("a", "b", 20);
            graph.AddEdge("b", "c", 1);
            graph.AddEdge("c", "a", 1);
            graph.AddEdge("b", "end", 30);

            Console.WriteLine("Test4");
            algo.DisplayMinimalCost(graph, "start", "end");
            Console.WriteLine();
        }
    }
}
