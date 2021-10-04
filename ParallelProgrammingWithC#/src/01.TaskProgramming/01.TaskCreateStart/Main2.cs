using System;
using System.Threading.Tasks;

namespace TaskCreateStart
{
    public static class Main2
    {
        public static void Execute()
        {
            var t = new Task(Write, "hello");
            t.Start();

            Task.Factory.StartNew(Write, 123);

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }

        public static void Write(object o)
        {
            var i = 1000;
            while (i-- > 0)
            {
                Console.Write(o);
            }
        }
    }
}
