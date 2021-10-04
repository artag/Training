using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingTasks
{
    public static class Main4
    {
        public static void Execute()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            token.Register(() =>
            {
                Console.WriteLine("Cancelation has been requested.");
            });

            var t = new Task(() =>
            {
                var i = 0;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
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
