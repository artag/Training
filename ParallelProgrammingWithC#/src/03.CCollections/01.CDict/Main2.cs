using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CDict
{
    public static class Main2
    {
        private static ConcurrentDictionary<string, string> capitals =
            new ConcurrentDictionary<string, string>();

        private static void AddOrUpdate(string newValue)
        {
            capitals.AddOrUpdate(
                "Russia", newValue, (key, oldvalue) => oldvalue + $" --> {newValue}");
        }

        public static void Execute()
        {
            //capitals["Russia"] = "Leningrad";       // Нормально добавится
            //capitals["Russia"] = "Moscow";          // Нормально обновится

            var task1 = Task.Factory.StartNew(() => AddOrUpdate("Leningrad"));
            var task2 = Task.Factory.StartNew(() => AddOrUpdate("Moscow"));

            Task.WaitAll(task1, task2);
            Console.WriteLine($"The capital of Russia is {capitals["Russia"]}");
        }
    }
}
