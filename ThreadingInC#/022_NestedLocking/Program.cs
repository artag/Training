public class Program
{
    public static void Main()
    {
        var a = new ThreadSafe();
        var thread = new Thread(() => a.Go("Thread"));

        a.Go("Main thread");
        thread.Start();

        Console.WriteLine($"End. Result = {a}");
    }
}

public class ThreadSafe
{
    private int _x = 0;
    private static readonly object _locker = new object();

    public override string ToString() =>
        _x.ToString();

    public void Go(string name)
    {
        lock (_locker)
        {
            Console.WriteLine($"{name}: Go. Enter lock");
            GoInternal(name);
            Console.WriteLine($"{name}: Go. Exit lock");
        }
    }

    private void GoInternal(string name)
    {
        lock (_locker)
        {
            Console.WriteLine($"{name}: GoInternal. Enter lock");

            for (int i = 0; i < 50; i++)
            {
                _x++;
            }

            Console.WriteLine($"{name}: GoInternal. Exit lock");
        }
    }
}
