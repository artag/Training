```csharp
public class Program
{
    private static readonly CountdownEvent _countdown =
        new CountdownEvent(initialCount: 3);

    public static void Main()
    {
        for (var i = 0; i < 3; i++)
        {
            var tmp = i;
            new Thread(() => Say($"I'm thread {tmp}")).Start();
        }

        _countdown.Wait();      // Blocks until Signal has been called 3 times.

        Console.WriteLine("All threads have finished speaking! Reset.");

        // Сброс, иначе будет исключение при вызове Signal() на пустом CountdownEvent
        _countdown.Reset();

        for (var i = 3; i < 6; i++)
        {
            var tmp = i;
            new Thread(() => Say($"I'm thread {tmp}")).Start();
        }

        _countdown.Wait();

        Console.WriteLine("All threads have finished speaking!");
    }

    private static void Say(string message)
    {
        Thread.Sleep(1000);
        Console.WriteLine(message);
        _countdown.Signal();
    }
}
```

Вывод:

```text
I'm thread 1
I'm thread 0
I'm thread 2
All threads have finished speaking! Reset.
I'm thread 3
I'm thread 4
I'm thread 5
All threads have finished speaking!
```
