class Program
{
    static readonly object _locker = new object();
    static bool _done;

    static void Main()
    {
        new Thread(Go).Start();
        Go();
    }

    private static void Go()
    {
        lock (_locker)
        {
            if (_done)
                return;

            Console.WriteLine("Done");
            _done = true;
        }
    }
}
