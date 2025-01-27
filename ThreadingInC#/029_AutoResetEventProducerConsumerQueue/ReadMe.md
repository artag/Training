```csharp
public class Program
{
    public static void Main()
    {
        using var pc = new ProducerConsumerQueue();

        pc.Produce("Hello");
        for (var i = 0; i < 10; i++)
        {
            pc.Produce("Say " + i);
            Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 4)));
        }
        pc.Produce("Goodbye!");

        // Finish
        Console.WriteLine("--- Finish Main");
    }
}

public class ProducerConsumerQueue : IDisposable
{
    private readonly EventWaitHandle _wh = new AutoResetEvent(initialState: false);
    private readonly object _locker = new object();
    private readonly Queue<string?> _queue = new Queue<string?>();
    private readonly Thread _worker;

    public ProducerConsumerQueue()
    {
        _worker = new Thread(() => Consume());
        _worker.Start();
    }

    public void Produce(string? value)
    {
        lock (_locker)
        {
            _queue.Enqueue(value);

            if (value == null)
                Console.WriteLine($"--- Produce finish signal");
            else
                Console.WriteLine($"--- Produce {value}");

            _wh.Set();
        }
    }

    public void Dispose()
    {
        Produce(null);      // Signal the consumer to exit.
        _worker.Join();     // Wait for the consumer's thread to finish.
        _wh.Close();
        Console.WriteLine("--- Finish Dispose.");
    }

    private void Consume()
    {
        while (true)
        {
            string? value = null;
            lock (_locker)
            {
                if (_queue.Count > 0)
                {
                    value = _queue.Dequeue();
                    if (value == null)
                        return;             // Consumer exit.
                }
            }

            if (value != null)
            {
                Console.WriteLine("Performing task: " + value);
                Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 11)));
            }
            else    // No more values, wait for signal.
            {
                Console.WriteLine("--- Waiting...");
                _wh.WaitOne();
            }
        }
    }
}
```

Вывод:

```text
--- Produce Hello
Performing task: Hello
--- Produce Say 0
--- Produce Say 1
--- Produce Say 2
--- Produce Say 3
--- Produce Say 4
Performing task: Say 0
--- Produce Say 5
--- Produce Say 6
--- Produce Say 7
--- Produce Say 8
--- Produce Say 9
--- Produce Goodbye!
--- Finish Main
--- Produce finish signal
Performing task: Say 1
Performing task: Say 2
Performing task: Say 3
Performing task: Say 4
Performing task: Say 5
Performing task: Say 6
Performing task: Say 7
Performing task: Say 8
Performing task: Say 9

Performing task: Goodbye!

--- Finish Dispose.
```
