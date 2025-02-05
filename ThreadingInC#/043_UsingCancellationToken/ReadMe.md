```csharp
public class Program
{
    public static void Main()
    {
        var cancelSource = new CancellationTokenSource();
        new Thread(() =>
        {
            try
            {
                Work(cancelSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Canceled");
            }
        }).Start();

        Thread.Sleep(1000);
        cancelSource.Cancel();

        Console.WriteLine("Main end");
    }

    private static void Work(CancellationToken cancelToken)
    {
        while (true)
        {
            cancelToken.ThrowIfCancellationRequested();

            Thread.Sleep(100);
            Console.WriteLine("Work. Do stuff...");
            try
            {
                OtherMethod(cancelToken);
            }
            finally
            {
                Console.WriteLine("Work. Cleanup");
            }
        }
    }

    private static void OtherMethod(CancellationToken cancelToken)
    {
        Thread.Sleep(200);
        Console.WriteLine("OtherMethod. Do stuff...");

        cancelToken.ThrowIfCancellationRequested();
    }
}
```

Вывод:

```text
Work. Do stuff...
OtherMethod. Do stuff...
Work. Cleanup
Work. Do stuff...
OtherMethod. Do stuff...
Work. Cleanup
Work. Do stuff...
OtherMethod. Do stuff...
Work. Cleanup
Main end
Work. Do stuff...
OtherMethod. Do stuff...
Work. Cleanup
Canceled
```
