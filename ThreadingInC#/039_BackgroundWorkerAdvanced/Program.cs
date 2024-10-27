using System.ComponentModel;

public class Program
{
    public static void Main()
    {
        using var bw = new BackgroundWorker()
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true,
        };

        bw.DoWork += bw_DoWork;
        bw.ProgressChanged += bw_ProgressChanged;
        bw.RunWorkerCompleted += bw_RunWorkerCompleted;

        var data = new Data(bw, "Hello to worker");
        bw.RunWorkerAsync(data);

        Console.WriteLine("Press Enter in the next 5 seconds to cancel");
        Console.ReadLine();
        if (bw.IsBusy)
        {
            Console.WriteLine("Sending cancel signal");
            bw.CancelAsync();
        }

        Console.ReadLine();
    }

    private static void bw_DoWork(object? sender, DoWorkEventArgs e)
    {
        var data = (Data)e.Argument!;
        var bw = data.BackgroundWorker;
        var msg = data.Message;

        for (int i = 0; i <= 100; i += 20)
        {
            if (bw.CancellationPending)
            {
                Console.WriteLine("Worker. Cancellation pending");
                e.Cancel = true;
                return;
            }

            bw.ReportProgress(percentProgress: i);
            Thread.Sleep(1000);     // Work emulation
        }

        e.Result = 123;     // This gets passed to RunWorkerCompleted
    }

    private static void bw_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        Console.WriteLine("Reached " + e.ProgressPercentage + "%");
    }

    private static void bw_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Cancelled)
            Console.WriteLine("You cancelled");
        else if (e.Error != null)
            Console.WriteLine("Worker exception: " + e.Error.ToString());
        else
            Console.WriteLine("Complete: " + e.Result);     // Result from DoWork
    }
}

internal record struct Data(
    BackgroundWorker BackgroundWorker,
    string Message
);
