```csharp
using System.ComponentModel;

public class Program
{
    public static void Main()
    {
        using var bw = new BackgroundWorker();
        bw.DoWork += bw_DoWork!;
        bw.RunWorkerCompleted += bw_DoWorkCompleted;
        bw.RunWorkerAsync("Message to worker");

        Console.WriteLine("Press 'Enter' to quit");
        Console.ReadLine();
    }

    private static void bw_DoWork(object sender, DoWorkEventArgs e)
    {
        // This is called on the worker thread
        Console.WriteLine("Says: " + e.Argument);      // says: "Message to worker"
        // Perform time-consuming task...
    }

    private static void bw_DoWorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        Console.WriteLine("Do some work after BackgroundWorker");
    }
}
```

Вывод:

```text
Press 'Enter' to quit
Says: Message to worker
Do some work after BackgroundWorker
```
