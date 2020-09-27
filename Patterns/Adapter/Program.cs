using System;

namespace Adapter
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new Source();
            var target = new Adapter(source);

            var result = target.GetRequest();
            Console.WriteLine($"Result: {result}");
        }
    }
}
