```csharp
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
    }
}

public class Foo
{
    private readonly object _lock = new object();
    private Expensive? _expensive;

    public Expensive Expensive
    {
        get
        {
            lock (_lock)
            {
                if (_expensive == null)
                    _expensive = new Expensive();
                return _expensive;
            }
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
```

Вывод:

```text
Constructing...
Constructing complete
Expensive. Run...
Expensive. Run...
Done
```
