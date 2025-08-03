**Not recommended use Thread as timer**

```csharp
public class Program
{
    public static void Main()
    {
        var enabled = true;
        ThreadPool.QueueUserWorkItem(data =>
        {
            while (enabled)
            {
                DoSomeAction();
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine("Timer stopped");
        });

        Console.WriteLine("Press 'Enter' to quit...");
        Console.ReadLine();

        enabled = false;
        Console.WriteLine("Done");
    }

    private static void DoSomeAction()
    {
        Console.WriteLine($"{DateTime.Now.ToString("ss, fff")}: tick");
    }
}

```

Вывод:

```text
Press 'Enter' to quit...
14, 227: tick
19, 268: tick
24, 279: tick
```
