```csharp
public class Program
{
    public static void Main()
    {
        var a = new A();
        Thread.CurrentThread.Name = "ThreadMain";

        var t1 = new Thread(() => a.Run(n => n * 3));
        var t2 = new Thread(() => a.Run(n => n * n));
        t1.Name = "Thread1";
        t2.Name = "Thread2";

        t1.Start();
        a.Run(n => n + 1);
        t2.Start();

        t1.Join();
        t2.Join();
        Console.WriteLine("Done");
    }
}

public class A
{
    private ThreadLocal<int> _num = new ThreadLocal<int>(() => 5);

    public void Run(Func<int, int> func)
    {
        var result = func(_num.Value);
        Console.WriteLine($"{Thread.CurrentThread.Name}: {result}");
    }
}
```

```text
ThreadMain: 6
Thread1: 15
Thread2: 25
Done
```
