using System;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    public static class Main4
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
                ae.Handle(e =>
                {
                    if (e is InvalidOperationException)
                    {
                        Console.WriteLine("Invalid op!");
                        return true; // true - исключение было обработано.
                    }

                    return false;   // false - исключение не было обработано.
                });
            }

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
