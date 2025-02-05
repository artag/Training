```csharp
public class Program
{
    public static void Main()
    {
        var t = new Thread(delegate()
        {
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException ex)
            {
                Console.WriteLine("Interrupted: {0}", ex.Message);
            }

            Console.WriteLine("Finish thread");
        });

        t.Start();
        t.Interrupt();
    }
}
```

Вывод:

```text
Interrupted: Thread was interrupted from a waiting state.
Finish thread
```
