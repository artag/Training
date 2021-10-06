using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLocks
{
    public static class Main4
    {
        public static void Execute()
        {
            var padLock = new ReaderWriterLockSlim();
            var random = new Random();

            int x = 0;

            var tasks = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                var taskNum = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padLock.EnterUpgradeableReadLock();
                    Console.WriteLine($"Entered read lock, x = {x}");

                    if (taskNum % 2 == 1)
                    {
                        padLock.EnterWriteLock();
                        x = random.Next(10);
                        Console.WriteLine($"Set {x}");
                        padLock.ExitWriteLock();
                    }

                    Thread.Sleep(2000);

                    padLock.ExitUpgradeableReadLock();
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

            Console.WriteLine("The end");
        }
    }
}
