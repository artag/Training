using System;
using System.Threading.Tasks;

namespace Continuations
{
    public static class Main3
    {
        public static void Execute()
        {
            var task1 = Task.Factory.StartNew(() => "Task 1");
            var task2 = Task.Factory.StartNew(() => "Task 2");

            var task3 = Task.Factory.ContinueWhenAny(
                new [] { task1, task2 },
                previousTask =>
                {
                    Console.WriteLine("Tasks completed:");
                    Console.WriteLine(" - " + previousTask.Result);
                    Console.WriteLine("All tasks done");
                });

            task3.Wait();
        }
    }
}
