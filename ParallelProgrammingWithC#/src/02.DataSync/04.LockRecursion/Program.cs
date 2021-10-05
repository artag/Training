using System;
using System.Threading;

namespace LockRecursion
{
    class Program
    {
        static SpinLock sl = new SpinLock();

        public static void LockRecursion(int x)
        {
            bool lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (LockRecursionException ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock, x = {x}");
                    LockRecursion(x - 1);
                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock, x = {x}");
                }
            }
        }

        static void Main(string[] args)
        {
            LockRecursion(5);
        }
    }
}
