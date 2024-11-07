public class Program
{
    public static void Main()
    {
        var waitHandles = new List<WaitHandle>();

        for (var i = 0; i < 10; i++)
        {
            var h1 = new EventWaitHandle(initialState: false, mode: EventResetMode.ManualReset);
            var h2 = new EventWaitHandle(initialState: false, mode: EventResetMode.ManualReset);

            waitHandles.Add(h1);
            waitHandles.Add(h2);

            new Thread(() => { ThreadUnsafe.Go(); h1.Set(); }).Start();
            new Thread(() => { ThreadSafe.Go(); h2.Set(); }).Start();
        }

        WaitHandle.WaitAll(waitHandles.ToArray());

        Console.WriteLine($"Unsafe decrement result = {ThreadUnsafe.X}");
        Console.WriteLine($"Safe decrement result = {ThreadSafe.X}");
    }
}

public static class ThreadUnsafe
{
    private static long _x = 1000L;

    public static long X => _x;

    public static void Go()
    {
        for (var i = 0; i < 100; i++)
        {
            _x--;
        }
    }
}

public static class ThreadSafe
{
    private static long _x = 1000L;

    public static long X => _x;

    public static void Go()
    {
        for (var i = 0; i < 100; i++)
        {
            Interlocked.Decrement(ref _x);
        }
    }
}
