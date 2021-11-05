using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    // Пример использования async/await.
    // В видео была показан пример в WinForms приложении.
    internal static class CalcAsync
    {
        private static async Task<int> CalculateValueAsync()
        {
            Console.WriteLine("Calculating async...");
            await Task.Delay(5000);
            Console.WriteLine("Calculated.");
            return 123;
        }

        public static async Task Execute()
        {
            var i = 0;
            Task<int> task = null;
            int result;

            Console.WriteLine("Press Enter to stop");
            while(!Console.KeyAvailable)
            {
                while (!Console.KeyAvailable)
                {
                    Console.WriteLine($"Working... {i}/9");
                    Thread.Sleep(1000);
                    if (i == 3)
                    {
                        task = CalculateValueAsync();
                        i++;
                    }
                    else if (i >= 10)
                    {
                        result = await task;
                        Console.WriteLine($"Calculated result = {result}");
                        i = 0;
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
