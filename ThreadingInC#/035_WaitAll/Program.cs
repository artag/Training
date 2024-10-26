public class Program
{
    private static readonly WaitHandle[] _waitHandles = new WaitHandle[]
    {
        new AutoResetEvent(initialState: false),
        new AutoResetEvent(initialState: false),
    };

    private static readonly Random _random = new Random();

    public static void Main()
    {
        var dt = DateTime.Now;
        ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask!), _waitHandles[0]);
        ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask!), _waitHandles[1]);

        WaitHandle.WaitAll(_waitHandles);
        var timeWasted = (DateTime.Now - dt).TotalMilliseconds;
        Console.WriteLine(
            "Both tasks are completed, time waited={0} ms", timeWasted);
    }

    private static void DoTask(object state)
    {
        var are = (AutoResetEvent)state;
        var time = 1000 * _random.Next(2, 10);
        Console.WriteLine($"Performing a task for {time} ms");
        Thread.Sleep(time);
        are.Set();
    }
}
