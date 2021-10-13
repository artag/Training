using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CDict
{
    public static class Main4
    {
        private static ConcurrentDictionary<string, string> capitals =
            new ConcurrentDictionary<string, string>();

        private static void TryRemove(string taskName, string keyToRemove)
        {
            string removed;
            var didRemove = capitals.TryRemove(keyToRemove, out removed);
            if (didRemove)
                Console.WriteLine($"Task {taskName}: We just removed {removed}.");
            else
                Console.WriteLine($"Task {taskName}: Failed to remove the capital of {keyToRemove}.");
        }

        public static void Execute()
        {
            capitals["Russia"] = "Moscow";

            var task1 = Task.Factory.StartNew(() => TryRemove("1", "Russia"));
            var task2 = Task.Factory.StartNew(() => TryRemove("2", "Russia"));

            Task.WaitAll(task1, task2);
        }
    }
}
