```csharp
public class Program
{
    static void Main()
    {
        var t = Task.Factory.StartNew(Go);

        Console.WriteLine("Hello from main thread");

        try
        {
            t.Wait();
        }
        catch (AggregateException ae)
        {
            ae.Handle(ex =>
            {
                if (ex is NullReferenceException nre)
                {
                    Console.WriteLine("Catch exception from task: " + nre.Message);
                    return true;
                }

                return false;
            });
        }

        Console.WriteLine("The end");
    }

    private static void Go()
    {
        Console.WriteLine("Hello from thread pool");
        throw new NullReferenceException("Exception from task");
    }
}
```

Вывод:

```text
Hello from main thread
Hello from thread pool
Catch exception from task: Exception from task
The end
```
