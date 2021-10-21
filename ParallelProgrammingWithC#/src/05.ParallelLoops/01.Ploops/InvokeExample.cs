using System;
using System.Threading.Tasks;

namespace Ploops
{
    public class InvokeExample
    {
        public static void Execute()
        {
            var a = new Action(() => Console.WriteLine($"First {Task.CurrentId}"));
            var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
            var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId}"));

            Parallel.Invoke(a, b, c);
            Console.WriteLine("The end.");
        }
    }
}
