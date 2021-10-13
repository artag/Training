using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CDict
{
    public static class Main1
    {
        private static ConcurrentDictionary<string, string> capitals =
            new ConcurrentDictionary<string, string>();

        private static void AddParis()
        {
            var success = capitals.TryAdd("France", "Paris");

            // Какой поток вызвал метод. CurrentId = null, если поток Main thread
            var who = Task.CurrentId.HasValue ? ("Task " + Task.CurrentId) : "Main thread";
            Console.WriteLine($"{who} {(success ? "added" : "did not add")} the element.");
        }

        public static void Execute()
        {
            var task = Task.Factory.StartNew(AddParis);
            AddParis();
            task.Wait();
        }
    }
}
