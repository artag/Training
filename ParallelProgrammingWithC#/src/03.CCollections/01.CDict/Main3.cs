using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CDict
{
    public static class Main3
    {
        private static ConcurrentDictionary<string, string> capitals =
            new ConcurrentDictionary<string, string>();

        private static void GetOrAdd(string newValue)
        {
            capitals.GetOrAdd("Sweden", newValue);
            Console.WriteLine($"The capital of Sweden is {capitals["Sweden"]}");
        }

        public static void Execute()
        {
            var task1 = Task.Factory.StartNew(() => GetOrAdd("Uppsala"));
            var task2 = Task.Factory.StartNew(() => GetOrAdd("Stockholm"));

            Task.WaitAll(task1, task2);
        }
    }
}
