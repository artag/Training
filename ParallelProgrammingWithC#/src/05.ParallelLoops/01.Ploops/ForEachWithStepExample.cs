using System.Drawing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploops
{
    public class ForEachWithStepExample
    {
        private static IEnumerable<int> Range(int start, int end, int step)
        {
            for (var i = start; i < end; i += step)
            {
                yield return i;
            }
        }

        public static void Execute()
        {
             Parallel.ForEach(
                Range(1, 20, 3),
                num => Console.WriteLine($"{num} from task {Task.CurrentId}"));

            Console.WriteLine($"The end.");
        }
    }
}
