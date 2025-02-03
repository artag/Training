```csharp
using System.ComponentModel;

public class Program
{
    public static void Main()
    {
        var client = new Client();
        using var worker = client.GetFinancialWorkerBackground(1, 2);
        worker.RunWorkerCompleted += (s, e) =>
        {
            if (e.Cancelled)
                Console.WriteLine("Cancelled operation");
            else if (e.Error != null)
                Console.WriteLine("Error: " + e.Error.ToString());
            else
                Console.WriteLine("Result: " + e.Result);
        };
        worker.ProgressChanged += (s, e) =>
        {
            Console.WriteLine($"{e.ProgressPercentage}%    {e.UserState}");
        };

        worker.RunWorkerAsync();

        Console.WriteLine("Press 'Enter' in the next 5 seconds to cancel");
        Console.ReadLine();
        if (worker.IsBusy)
            worker.CancelAsync();
        Console.ReadLine();
    }
}

public class Client
{
    public FinancialWorker GetFinancialWorkerBackground(int foo, int bar)
    {
        return new FinancialWorker(foo, bar);
    }
}

public class FinancialWorker : BackgroundWorker
{
    public FinancialWorker(int foo, int bar) : this()
    {
        Foo = foo;
        Bar = bar;
    }

    public FinancialWorker()
    {
        WorkerReportsProgress = true;
        WorkerSupportsCancellation = true;
    }

    public int Foo { get; }
    public int Bar { get; }

    protected override void OnDoWork(DoWorkEventArgs e)
    {
        for (var i = 0; i <= 90; i += 10)
        {
            if (CancellationPending)
            {
                Console.WriteLine("Cancel operation");
                e.Cancel = true;
                return;
            }

            Thread.Sleep(500);
            ReportProgress(i, "Working");
        }
        ReportProgress(100, "Done!");
        e.Result = 42;
    }
}
```

Вывод, если не прерывать выполнение:

```text
Press 'Enter' in the next 5 seconds to cancel
0%    Working
10%    Working
20%    Working
30%    Working
40%    Working
50%    Working
60%    Working
70%    Working
80%    Working
90%    Working
100%    Done!
Result: 42
```

Вывод, если прервать выполнение:

```text
Press 'Enter' in the next 5 seconds to cancel
0%    Working
10%    Working
20%    Working
30%    Working
40%    Working
50%    Working

Cancel operation
60%    Working
Cancelled operation
```
