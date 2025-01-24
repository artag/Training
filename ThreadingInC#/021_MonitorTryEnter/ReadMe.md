```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var sleepTimeMs = args.Length == 0 ? 0 : 500;
        var thread = new Thread(() => ThreadUnsafe.Go("Thread", sleepTimeMs));

        thread.Start();
        ThreadUnsafe.Go("Main thread", sleepTimeMs);
    }
}

public static class ThreadUnsafe
{
    static readonly object _locker = new object();
    static readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(50);

    static int _val1 = 1;
    static int _val2 = 1;

    public static void Go(string name, int sleepTimeMs)
    {
        var lockTaken = false;
        try
        {
            Monitor.TryEnter(_locker, _timeout, ref lockTaken);
            if (lockTaken)
            {
                Console.WriteLine($"{name}: Lock acquired");

                Thread.Sleep(sleepTimeMs);

                if (_val2 != 0)
                    Console.WriteLine($"    Result from {name}: {_val1 / _val2}");
                _val2 = 0;
            }
            else
            {
                Console.WriteLine($"{name}: Lock not acquired");
            }
        }
        finally
        {
            if (lockTaken)
                {
                    Monitor.Exit(_locker);
                    Console.WriteLine($"{name}: Release lock");
                }
        }
    }
}
```

## Вывод

### 1. В командной строке нет аргументов

Может быть так:

```text
Thread: Lock acquired
    Result from Thread: 1
Thread: Release lock
Main thread: Lock acquired
Main thread: Release lock
```

А может быть так:

```text
Thread: Lock acquired
Main thread: Lock not acquired
    Result from Thread: 1
Thread: Release lock
```

### 2. В командной строке есть хотя бы один аргумент

```text
Main thread: Lock acquired
Thread: Lock not acquired
    Result from Main thread: 1
Main thread: Release lock
```
