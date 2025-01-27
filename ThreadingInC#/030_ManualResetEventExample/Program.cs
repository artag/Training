public class Program
{
    private static ManualResetEvent mre = new ManualResetEvent(initialState: false);

    public static void Main()
    {
        for (var i = 0; i < 3; i++)
        {
            var t = new Thread(() => ThreadProc());
            t.Name = "Thread_" + i;
            t.Start();
        }

        Thread.Sleep(500);
        Console.WriteLine("Press 'Enter' to call Set()");
        Console.ReadLine();

        mre.Set();

        Console.WriteLine("Press 'Enter' to continue");
        Console.ReadLine();
        for (var i = 3; i < 5; i ++)
        {
            var t = new Thread(() => ThreadProc());
            t.Name = "Thread_" + i;
            t.Start();
        }

        Thread.Sleep(500);
        Console.WriteLine("Press 'Enter' to call Reset()");
        Console.ReadLine();

        mre.Reset();

        Thread t5 = new Thread(ThreadProc);
        t5.Name = "Thread_5";
        t5.Start();

        Thread.Sleep(500);
        Console.WriteLine("\nPress Enter to call Set()");
        Console.ReadLine();

        mre.Set();
    }

    private static void ThreadProc()
    {
        var name = Thread.CurrentThread.Name;
        Console.WriteLine(name + " starts");

        mre.WaitOne();

        Console.WriteLine(name + " ends");
    }
}
