using System;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    public static class Main3
    {
        public static void Execute()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Running task 1...");
                throw new InvalidOperationException("Can't do this!") { Source = "t1"};
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Running task 2...");
                throw new AccessViolationException("Can't access this!") { Source = "t2"};
            });

            try
            {
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                    Console.WriteLine($"Exception {e.GetType()} from {e.Source}");
            }

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
