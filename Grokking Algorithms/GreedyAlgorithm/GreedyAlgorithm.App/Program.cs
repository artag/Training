using System;
using System.Collections.Generic;

namespace GreedyAlgorithm.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var stations = new HashSet<Station>();
            stations.Add(new Station("kone", new[] { "id", "nv", "ut" }));
            stations.Add(new Station("ktwo", new[] { "wa", "id", "mt" }));
            stations.Add(new Station("kthree", new[] { "or", "nv", "ca" }));
            stations.Add(new Station("kfour", new[] { "nv", "ut" }));
            stations.Add(new Station("kfive", new[] { "ca", "az" }));

            var states = new HashSet<string> { "mt", "wa", "or", "id", "nv", "ut", "ca", "az" };
            DisplayStates(states);

            var selectedStations = Algorithm.SelectBestStations(stations, states);
            DisplayStations(selectedStations);
        }

        private static void DisplayStates(IEnumerable<string> states)
        {
            Console.Write("States: ");

            foreach (var state in states)
                Console.Write(state + " ");

            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DisplayStations(IEnumerable<Station> stations)
        {
            foreach (var station in stations)
            {
                Console.WriteLine($"Station: {station.Name}");
                Console.Write($"Covered states: ");
                foreach (var state in station.CoveredStates)
                    Console.Write(state + " ");
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
