public class Program
{
    private static readonly object _lock = new object();

    // Эффект deadlock заметен только в Release.
    public static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "0")
            RunDeadlockExample();
        else if (args[0] == "1")
            RunFixWithMemoryBarrier();
        else
            RunFixWithLock();
    }

    private static void RunDeadlockExample()
    {
        Console.WriteLine("Deadlock example");
        var complete = false;
        var t = new Thread(() =>
        {
            var toggle = false;
            while (!complete)
            {
                toggle = !toggle;
            }
        });

        t.Start();
        Thread.Sleep(1000);

        Console.WriteLine("Set complete = true");
        complete = true;
        t.Join();                   // Blocks indefinitely
        Console.WriteLine("Done");
    }

    private static void RunFixWithMemoryBarrier()
    {
        Console.WriteLine("Fix deadlock with MemoryBarrier");
        var complete = false;
        var t = new Thread(() =>
        {
            var toggle = false;
            Thread.MemoryBarrier();
            while (!complete)
            {
                Thread.MemoryBarrier();
                toggle = !toggle;
            }
        });

        t.Start();
        Thread.Sleep(1000);

        Console.WriteLine("Set complete = true");
        complete = true;
        t.Join();
        Console.WriteLine("Done");
    }

        private static void RunFixWithLock()
    {
        Console.WriteLine("Fix deadlock with lock");
        var complete = false;
        var t = new Thread(() =>
        {
            var toggle = false;
            while (true)
            {
                lock (_lock)
                {
                    if (complete)
                        break;
                }

                toggle = !toggle;
            }
        });

        t.Start();
        Thread.Sleep(1000);

        Console.WriteLine("Set complete = true");
        complete = true;
        t.Join();
        Console.WriteLine("Done");
    }
}
