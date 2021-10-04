using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingTasks
{
    public static class Main2
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
                        throw new OperationCanceledException();

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
