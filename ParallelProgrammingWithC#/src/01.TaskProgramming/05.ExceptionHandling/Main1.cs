using System;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    public static class Main1
    {
        public static void Execute()
        {
            var t = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Running task...");
                throw new InvalidOperationException();
            });

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
