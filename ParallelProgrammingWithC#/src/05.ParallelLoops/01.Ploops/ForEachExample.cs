using System;
using System.Threading.Tasks;

namespace Ploops
{
    public class ForEachExample
    {
        public static void Execute()
        {
            var words = new[] { "oh", "what", "a", "night" };
            Parallel.ForEach(words, word =>
            {
                Console.WriteLine($"\"{word}\" has length {word.Length} (task {Task.CurrentId})");
            });

            Console.WriteLine($"The end.");
        }
    }
}
