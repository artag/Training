```csharp
public class Program
{
    private static readonly WaitHandle[] _waitHandles = new WaitHandle[]
    {
        new AutoResetEvent(initialState: false),
        new AutoResetEvent(initialState: false),
    };

    private static readonly Random _random = new Random();

    public static void Main()
    {
        var dt = DateTime.Now;
        ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask!), _waitHandles[0]);
        ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask!), _waitHandles[1]);

        var index = WaitHandle.WaitAny(_waitHandles);
        Console.WriteLine(
            "Task {0} finished first, time waited={1} ms",
            index, (DateTime.Now - dt).TotalMilliseconds);
    }

    private static void DoTask(object state)
    {
        var are = (AutoResetEvent)state;
        var time = 1000 * _random.Next(2, 10);
        Console.WriteLine($"Performing a task for {time} ms");
        Thread.Sleep(time);
        are.Set();
    }
}
```

Вывод:

```text
Performing a task for 8000 ms
Performing a task for 8000 ms
Task 1 finished first, time waited=8062.7018 ms
```
