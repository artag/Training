using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingTasks
{
    public static class Main6
    {
        public static void Execute()
        {
            var planned = new CancellationTokenSource();
            var preventative = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            // Объединение token source.
            // Работа задачи будет прервана, если сработает один из token'ов.
            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(
                planned.Token, preventative.Token, emergency.Token);

            Task.Factory.StartNew(() =>
            {
                var i = 0;
                while (true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}");
                    Thread.Sleep(1000);
                }
            }, paranoid.Token);

            Console.ReadKey();
            // Вызовет прерывание работы задачи.
            // Этот прием также сработает на planned или preventative.
            emergency.Cancel();

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
