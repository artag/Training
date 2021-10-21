using System.Threading;
using System.Threading.Tasks;
using System;

namespace Cancel
{
    class Program
    {
        private static void ExecuteAndStop()
        {
            Parallel.For(0, 20, (x, state) =>
            {
                Console.WriteLine($"{x}. Task {Task.CurrentId}");
                if (x == 10)
                    state.Stop();
            });
        }

        private static void ExecuteAndBreak()
        {
            var result = Parallel.For(0, 20, (x, state) =>
            {
                Console.WriteLine($"{x}. Task {Task.CurrentId}");
                if (x == 10)
                    state.Break();
            });

            Console.WriteLine();
            Console.WriteLine($"Was loop completed? {result.IsCompleted}");
            if (result.LowestBreakIteration.HasValue)
                Console.WriteLine($"Lowest break iteration is {result.LowestBreakIteration}");
            // Выведет:
            //   Was loop completed? False
            //   Lowest break iteration is 10
        }

        private static void ExecuteAndThrow()
        {
            Parallel.For(0, 20, (x, state) =>
            {
                Console.WriteLine($"{x}. Task {Task.CurrentId}");
                if (x == 10)
                    throw new Exception("Breaking the parallel loop");
            });
        }

        private static void ExecuteAndCts()
        {
            var cts = new CancellationTokenSource();
            var po = new ParallelOptions();
            po.CancellationToken = cts.Token;

            Parallel.For(0, 20, po, x =>
            {
                Console.WriteLine($"{x}. Task {Task.CurrentId}");
                if (x == 10)
                    cts.Cancel();
            });
        }

        static void Main(string[] args)
        {
            // 1. Остановка при помощи Stop().
            // ExecuteAndStop();

            // 2. Остановка при помощи Break().
            // ExecuteAndBreak();

            // 3. Остановка при помощи исключения (любого).
            // try
            // {
            //     ExecuteAndThrow();
            // }
            // catch (AggregateException ae)
            // {
            //     ae.Handle(e =>
            //     {
            //         Console.WriteLine(e.Message);
            //         return true;
            //     });
            // }

            // 4. Остановка при помощи CancellationTokenSource.
            try
            {
                ExecuteAndCts();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
