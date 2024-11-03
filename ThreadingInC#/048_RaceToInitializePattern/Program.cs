using System.Threading;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var foo = new Foo();
        var t1 = new Thread(() => foo.Expensive.Run());
        var t2 = new Thread(() => foo.Expensive.Run());
        var t3 = new Thread(() => foo.Expensive.Run());
        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        t3.Start();
        t3.Join();
        Console.WriteLine("Done");

        // Display (analog LazyInitializer):
        // Constructing...          // t1
        // Constructing...          // t2
        // Constructing complete    // t1
        // Constructing complete    // t2
        // Expensive. Run...        // t1
        // Expensive. Run...        // t2
        // Expensive. Run...        // t3
        // Done
    }
}

public class Foo
{
    private readonly object _locker = new object();
    private volatile Expensive? _expensive;

    public Expensive Expensive
    {
        get
        {
            if (_expensive == null)
            {
                var instance = new Expensive();
                Interlocked.CompareExchange(
                    location1: ref _expensive,
                    value: instance,
                    comparand: null);
            }

            return _expensive;
        }
    }
}

public class Expensive
{
    public Expensive()
    {
        Console.WriteLine("Constructing...");
        Thread.Sleep(1000);
        Console.WriteLine("Constructing complete");
    }

    public void Run()
    {
        Console.WriteLine("Expensive. Run...");
    }
}
