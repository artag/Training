using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TwoMutex
{
   public class BankAccount
    {
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }

        public void Transfer(BankAccount where, int amount)
        {
            Withdraw(amount);
            where.Deposit(amount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();

            var ba1 = new BankAccount();
            var ba2 = new BankAccount();

            var mutex1 = new Mutex();
            var mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                // Пополнение первого банковского счета.
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var haveLock = mutex1.WaitOne();
                        try
                        {
                            ba1.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock)
                                mutex1.ReleaseMutex();
                        }
                    }
                }));

                // Пополнение второго банковского счета.
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var haveLock = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock)
                                mutex2.ReleaseMutex();
                        }
                    }
                }));

                // Перевод денег с первого банковского счета на второй.
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var haveLock = WaitHandle.WaitAll(new[]{ mutex1, mutex2 });
                        try
                        {
                            ba1.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex1.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Final balance ba1 is {ba1.Balance}.");
            Console.WriteLine($"Final balance ba2 is {ba2.Balance}.");
        }
    }
}
