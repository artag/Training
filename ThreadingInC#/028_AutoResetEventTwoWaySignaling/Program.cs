public class Program
{
    private static readonly object _locker = new object();

    public static string? Message { get; set; }

    public static void Main()
    {
        using var ready = new AutoResetEvent(initialState: false);
        using var go = new AutoResetEvent(initialState: false);

        var tws = new TwoWaySignaling(ready, go);
        new Thread(() => tws.Work()).Start();

        ready.WaitOne();        // First wait until worker is ready.
        lock (_locker)
            Message = "ooo";
        go.Set();               // Tell worker to go.

        ready.WaitOne();        // Wait until worker is ready.
        lock (_locker)
            Message = "ahhh";
        go.Set();               // Tell worker to go.

        ready.WaitOne();
        lock (_locker)
            Message = null;     // Signal the worker to exit.
        go.Set();

        Console.WriteLine("--- Main done");
    }
}

public class TwoWaySignaling
{
    private readonly EventWaitHandle _go;
    private readonly EventWaitHandle _ready;

    public TwoWaySignaling(EventWaitHandle ready, EventWaitHandle go)
    {
        _ready = ready;
        _go = go;
    }

    public void Work()
    {
        while (true)
        {
            _ready.Set();       // Worker ready.
            _go.WaitOne();

            if (Program.Message == null)
                break;

            Console.WriteLine(Program.Message);
        }

        Console.WriteLine("--- Worker done");
    }
}
