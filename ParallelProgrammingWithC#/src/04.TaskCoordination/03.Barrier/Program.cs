using System;
using System.Threading;
using System.Threading.Tasks;

namespace Barrier
{
    internal class Program
    {
        static System.Threading.Barrier barrier = new System.Threading.Barrier(
            participantCount: 2, postPhaseAction: b =>
        {
            Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished");
        });

        public static void Water()
        {
            Console.WriteLine("Phase 0. Putting the kettle on (takes a bit longer)");
            Thread.Sleep(2000);
            barrier.SignalAndWait();
            Console.WriteLine("Phase 1. Pouring water into cup");
            barrier.SignalAndWait();
            Console.WriteLine("Phase 2. Putting the kettle away");
        }

        public static void Cup()
        {
            Console.WriteLine("Phase 0. Finding the nicest cup of tea (fast)");
            barrier.SignalAndWait();
            Console.WriteLine("Phase 1. Adding tea");
            barrier.SignalAndWait();
            Console.WriteLine("Phase 2. Adding sugar");
        }

        static void Main(string[] args)
        {
            var water = Task.Factory.StartNew(Water);
            var cup = Task.Factory.StartNew(Cup);

            var tea = Task.Factory.ContinueWhenAll(new [] { water, cup }, tasks =>
            {
                Console.WriteLine("Enjoy your cup of tea.");
            });

            tea.Wait();
        }
    }
}
