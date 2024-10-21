﻿public class Program
{
    public static void Main()
    {
        var thread = new Thread(ThreadUnsafe.Go);

        thread.Start();
        ThreadUnsafe.Go();

        Console.WriteLine("End");
    }
}

public static class ThreadUnsafe
{
    static readonly object _locker = new object();
    static int _val1 = 1;
    static int _val2 = 1;

    public static void Go()
    {
        lock(_locker)
        {
            if (_val2 != 0)
                Console.WriteLine(_val1 / _val2);
            _val2 = 0;
        }
    }
}
