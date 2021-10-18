using System;
using System.Threading.Tasks;

namespace Continuations
{
    public static class Main1
    {
        public static void Execute()
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Boiling water");
            });

            var task2 = task.ContinueWith(t =>
            {
                Console.WriteLine($"Completed task {t.Id}, pour water into cup.");
            });

            task2.Wait();
        }
    }
}
