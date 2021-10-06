using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLocks
{
    public static class Main2
    {
        public static void Execute()
        {
            var padLock = new ReaderWriterLockSlim();
            var random = new Random();

            int x = 0;

            var tasks = new List<Task>();
            for (var i = 0; i < 5; i++)
            {
                var taskNum = i;
                tasks.Add(Task.Run(() =>
                {
                    ReadValue(padLock, x, taskNum);
                }));

                tasks.Add(Task.Run(() =>
                {
                    WriteRandomValue(padLock, random, ref x, taskNum);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("Press \"Enter\" to quit");
            Console.ReadKey();
        }

        private static void ReadValue(ReaderWriterLockSlim padLock, int x, int taskNum)
        {
            padLock.EnterReadLock();
            Console.WriteLine($"Entered read{taskNum} lock, x = {x}");

            Thread.Sleep(1000);

            Console.WriteLine($"Exited read{taskNum} lock, x = {x}");
            padLock.ExitReadLock();
        }

        private static void WriteRandomValue(ReaderWriterLockSlim padLock, Random random, ref int x, int taskNum)
        {
            padLock.EnterWriteLock();
            Console.WriteLine($"Entered write{taskNum} lock, x = {x}");

            var newValue = random.Next(10);
            x = newValue;
            Console.WriteLine($"Set{taskNum} x = {x}");
            Thread.Sleep(1000);

            Console.WriteLine($"Exited write{taskNum} lock, x = {x}");
            padLock.ExitWriteLock();
        }
    }
}