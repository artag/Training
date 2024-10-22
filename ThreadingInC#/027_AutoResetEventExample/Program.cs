public class Program
{
    public static void Main()
    {
        EventWaitHandle waitHandle = new AutoResetEvent(initialState: false);

        var bwh = new BasicWaitHandle(waitHandle);
        new Thread(() => bwh.Waiter()).Start();

        Console.WriteLine("Main. Start sleeping...");
        Thread.Sleep(TimeSpan.FromSeconds(2));
        Console.WriteLine("Main. End sleeping");

        waitHandle.Set();                           // Wake up waiter.

        Thread.Sleep(TimeSpan.FromSeconds(1));

        Console.WriteLine("Main. Start sleeping again...");
        waitHandle.Set();
        Console.WriteLine("Main. End sleeping again");

        new Thread(() => bwh.Waiter()).Start();     // No waiting
    }
}

public class BasicWaitHandle
{
    private readonly EventWaitHandle _waitHandle;

    public BasicWaitHandle(EventWaitHandle waitHandle)
    {
        _waitHandle = waitHandle;
    }

    public void Waiter()
    {
        Console.WriteLine("Waiting...");
        _waitHandle.WaitOne();
        Console.WriteLine("Notified");
    }
}
