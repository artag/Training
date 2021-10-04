using System;
using System.Threading;
using System.Threading.Tasks;

namespace WaitingForTasks
{
    public static class Main2
    {
        public static void Execute()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t1 = new Task(() =>
            {
                Console.WriteLine("I take 5 seconds");
                for (var i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }

                Console.WriteLine("I'm done");
            }, token);
            t1.Start();

            var t2 = Task.Run(() => Thread.Sleep(3000), token);

            Console.ReadKey();
            cts.Cancel();

            Task.WaitAll(new [] { t1, t2} , 4000, token);

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
