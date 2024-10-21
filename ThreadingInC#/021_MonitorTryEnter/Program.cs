public class Program
{
    public static void Main()
    {
        var thread = new Thread(() => ThreadUnsafe.Go("Thread"));

        thread.Start();
        ThreadUnsafe.Go("Main thread");

        Console.WriteLine("End");
    }
}

public static class ThreadUnsafe
{
    static readonly object _locker = new object();
    static readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(50);

    static int _val1 = 1;
    static int _val2 = 1;

    public static void Go(string name)
    {
        var lockTaken = false;
        try
        {
            Monitor.TryEnter(_locker, _timeout, ref lockTaken);
            if (lockTaken)
            {
                Console.WriteLine($"{name}: Lock acquired");

                if (_val2 != 0)
                    Console.WriteLine(_val1 / _val2);
                _val2 = 0;
            }
            else
            {
                Console.WriteLine($"{name}: Lock not acquired");
            }
        }
        finally
        {
            if (lockTaken)
                Monitor.Exit(_locker);
        }
    }
}
