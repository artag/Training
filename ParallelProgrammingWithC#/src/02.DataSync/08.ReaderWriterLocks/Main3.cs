using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLocks
{
    public static class Main3
    {
        public static void Execute()
        {
            var padLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            var random = new Random();

            int x = 0;

            var tasks = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padLock.EnterReadLock();
                    padLock.EnterReadLock();
                    Console.WriteLine($"Entered read lock, x = {x}");

                    Thread.Sleep(5000);

                    padLock.ExitReadLock();
                    padLock.ExitReadLock();
                    Console.WriteLine($"Exited read lock, x = {x}");
                }));
            }

            try
            {
                 Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();

                padLock.EnterWriteLock();
                Console.WriteLine("Write lock acquired");

                var newValue = random.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");

                padLock.ExitWriteLock();
                Console.WriteLine("Write lock released");
            }
        }
    }
}
