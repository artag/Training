using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventSlimDemo
{
    public static class Main2
    {
        public static void Execute()
        {
            var evt = new ManualResetEvent(initialState: false);  // 1) evt -> false

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Boiling water");
                evt.Set();                                      // 2) evt -> true
            });

            var makeTea = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Waiting for water...");
                evt.WaitOne();                                  // 3) evt -> true
                Console.WriteLine("Here is your tea");
                var ok = evt.WaitOne(1000);                     // ok = true (evt = true)
                if (ok)
                    Console.WriteLine("Enjoy your tea");
                else
                    Console.WriteLine("No tea for you");        // Это сработает
            });

            makeTea.Wait();
        }
    }
}
