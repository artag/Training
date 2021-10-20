using System;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSlimDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var semaphore = new SemaphoreSlim(initialCount: 2, maxCount: 10);

            for (int i = 0; i < 20; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task {Task.CurrentId}");
                    semaphore.Wait();       // ReleaseCount--
                    Console.WriteLine($"Processing task {Task.CurrentId}");
                });
            }

            while (semaphore.CurrentCount <= 2)
            {
                Console.WriteLine($"Semaphore count: {semaphore.CurrentCount}");
                Console.ReadKey();
                semaphore.Release(releaseCount: 2);     // ReleaseCount += 2
            }
        }
    }
}
