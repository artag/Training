using System;
using System.Threading;

namespace AsyncAwait
{
    // Пример блокировки потока в консоли. В видео была показана блокировка
    // при работе в WinForms приложении.
    internal static class CalcSync
    {
        private static int CalculateValue()
        {
            Console.WriteLine("Calculating...");
            Thread.Sleep(5000);
            return 123;
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
                    Console.WriteLine($"Working... {i}/3");
                    Thread.Sleep(1000);
                    if (i >= 3)
                    {
                        result = CalculateValue();
                        i = 0;
                        Console.WriteLine($"Calculated result = {result}");
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
