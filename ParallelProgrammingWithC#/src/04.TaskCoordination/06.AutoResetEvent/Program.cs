using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoResetEventDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var evt = new AutoResetEvent(initialState: false);  // 1) evt -> false

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Boiling water");
                evt.Set();                                      // 2) evt -> true
            });

            var makeTea = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Waiting for water...");
                evt.WaitOne();                                  // 3) evt -> false
                Console.WriteLine("Here is your tea");
                var ok = evt.WaitOne(1000);                     // ok = false (evt = false)
                if (ok)
                    Console.WriteLine("Enjoy your tea");
                else
                    Console.WriteLine("No tea for you");        // Это сработает
            });

            makeTea.Wait();
        }
    }
}
