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
        // Толком непонятно, что делает WaitHandle.SignalAndWait

        var dt = DateTime.Now;
        ThreadPool.QueueUserWorkItem(
            callBack: new WaitCallback(DoTask!),
            state: new Data(_waitHandles[0], "Task #1"));
        ThreadPool.QueueUserWorkItem(
            callBack: new WaitCallback(DoTask!),
            state: new Data(_waitHandles[1], "Task #2"));

        WaitHandle.SignalAndWait(_waitHandles[1], _waitHandles[0]);

        var timeWasted = (DateTime.Now - dt).TotalMilliseconds;
        Console.WriteLine(
            "Both tasks are completed, time waited={0} ms", timeWasted);

        Console.WriteLine("Press 'Enter' to quit");
        Console.ReadKey();
    }

    private static void DoTask(object state)
    {
        var data = (Data)state;
        var are = (AutoResetEvent)data.AutoResetEvent;
        var name = data.Name;

        var time = 1000 * _random.Next(2, 10);
        Console.WriteLine($"Performing a {name} for {time} ms");
        Thread.Sleep(time);
        are.Set();
        Console.WriteLine($"Finish {name}");
    }
}

internal record Data(
    WaitHandle AutoResetEvent,
    string Name
);
```

Вывод 1:

```text
Performing a Task #2 for 4000 ms
Performing a Task #1 for 7000 ms
Finish Task #2
Finish Task #1
Both tasks are completed, time waited=7126.7035 ms
Press 'Enter' to quit
```

Вывод 2:

```text
Performing a Task #1 for 5000 ms
Performing a Task #2 for 7000 ms
Finish Task #1
Both tasks are completed, time waited=5063.6624 ms
Press 'Enter' to quit
Finish Task #2
```
