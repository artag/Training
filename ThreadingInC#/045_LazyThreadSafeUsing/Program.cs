public class Program
{
    public static void Main()
    {
        var foo = new Foo();
        var t1 = new Thread(() => foo.Expensive.Run());
        var t2 = new Thread(() => foo.Expensive.Run());
        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
        Console.WriteLine("Done");

        // Display:
        // Constructing...
        // Constructing complete
        // Expensive. Run...
        // Expensive. Run...
        // Done
    }
}

public class Foo
{
    private Lazy<Expensive> _expensive = new Lazy<Expensive>(
        () => new Expensive(), isThreadSafe: true);

    public Expensive Expensive => _expensive.Value;
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
