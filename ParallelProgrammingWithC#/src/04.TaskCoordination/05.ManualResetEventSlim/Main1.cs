using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventSlimDemo
{
    public static class Main1
    {
        public static void Execute()
        {
            var evt = new ManualResetEventSlim();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Boiling water");
                evt.Set();
            });

            var makeTea = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Waiting for water...");
                evt.Wait();
                Console.WriteLine("Here is your tea");
            });

            makeTea.Wait();
        }
    }
}
