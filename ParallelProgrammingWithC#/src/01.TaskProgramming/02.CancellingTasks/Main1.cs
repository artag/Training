using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingTasks
{
    public static class Main1
    {
        public static void Execute()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var t = new Task(() =>
            {
                var i = 0;
                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;

                    Console.WriteLine($"{i++}");
                }
            }, token);
            t.Start();

            Console.ReadKey();
            cts.Cancel();

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
