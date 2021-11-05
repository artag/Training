using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    // Пример разруливания блокировки потока в консоли через использование Task и TaskContinuation.
    // В видео была показан пример в WinForms приложении.
    internal static class CalcTask
    {
        private static Task<int> CalculateValueAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Calculating async...");
                Thread.Sleep(5000);
                return 123;
            });
        }

        public static void Execute()
        {
            var i = 0;
            int result;

            Console.WriteLine("Press Enter to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    Console.WriteLine($"Working... {i}/9");
                    Thread.Sleep(1000);
                    if (i >= 9)
                    {
                        var calc = CalculateValueAsync();
                        i = 0;
                        calc.ContinueWith(t =>
                        {
                            result = t.Result;
                            Console.WriteLine($"Calculated result = {result}");
                        });
                    }
                    else
                    {
                        i++;
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
        }
    }
}
