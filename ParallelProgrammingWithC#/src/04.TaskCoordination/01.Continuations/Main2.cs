using System;
using System.Threading.Tasks;

namespace Continuations
{
    public static class Main2
    {
        public static void Execute()
        {
            var task1 = Task.Factory.StartNew(() => "Task 1");
            var task2 = Task.Factory.StartNew(() => "Task 2");

            var task3 = Task.Factory.ContinueWhenAll(
                new [] { task1, task2 },
                tasks =>
                {
                    Console.WriteLine("Tasks completed:");
                    foreach (var t in tasks)
                        Console.WriteLine(" - " + t.Result);
                    Console.WriteLine("All tasks done");
                });

            task3.Wait();
        }
    }
}
