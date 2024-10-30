public class Program
{
    public static void Main()
    {
        var canceler = new RulyCanceler();
        new Thread(() =>
        {
            try
            {
                Work(canceler);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Canceled");
            }
        }).Start();

        Thread.Sleep(1000);
        canceler.Cancel();

        Console.WriteLine("Main end");
    }

    private static void Work(RulyCanceler c)
    {
        while (true)
        {
            c.ThrowIfCancellationRequested();

            Thread.Sleep(100);
            Console.WriteLine("Work. Do stuff...");
            try
            {
                OtherMethod(c);
            }
            finally
            {
                Console.WriteLine("Work. Cleanup");
            }
        }
    }

    private static void OtherMethod(RulyCanceler c)
    {
        Thread.Sleep(200);
        Console.WriteLine("OtherMethod. Do stuff...");

        c.ThrowIfCancellationRequested();
    }
}

public class RulyCanceler
{
    private readonly object _cancelLocker = new object();
    bool _cancelRequest;

    public bool IsCancelledRequested
    {
        get
        {
            lock (_cancelLocker)
                return _cancelRequest;
        }
    }

    public void Cancel()
    {
        lock (_cancelLocker)
            _cancelRequest = true;
    }

    public void ThrowIfCancellationRequested()
    {
        if (IsCancelledRequested)
            throw new OperationCanceledException();
    }
}
