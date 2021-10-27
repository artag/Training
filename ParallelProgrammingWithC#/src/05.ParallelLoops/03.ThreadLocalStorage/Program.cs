using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadLocalStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            var sum = 0;
            Parallel.For(fromInclusive: 1, toExclusive: 1001,
            localInit: () => 0,
            body: (x, state, tls) =>
            {
                tls += x;
                //Console.WriteLine($"Task {Task.CurrentId} has sum {tls}");
                return tls;
            },
            localFinally: partialSum =>
            {
                //Console.WriteLine($"Partial value of task {Task.CurrentId} is {partialSum}");
                Interlocked.Add(ref sum, partialSum);
            });

            Console.WriteLine(sum);
        }
    }
}
