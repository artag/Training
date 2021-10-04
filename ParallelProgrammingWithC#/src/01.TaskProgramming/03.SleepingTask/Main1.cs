using System;
using System.Threading;
using System.Threading.Tasks;

namespace SleepingTask
{
    public static class Main1
    {
        public static void Execute()
        {
            var t = new Task(() =>
            {
                Thread.Sleep(1000);
            });

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
