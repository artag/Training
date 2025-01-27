public class Program
{
    private static ManualResetEvent _starter = new ManualResetEvent(initialState: false);

    public static void Main()
    {
        var reg = ThreadPool.RegisterWaitForSingleObject(
            waitObject: _starter,
            callBack: Go!,
            state: "Data passed to object",
            millisecondsTimeOutInterval: -1,
            executeOnlyOnce: true);

        Console.WriteLine("Main. Do some work");
        Thread.Sleep(3000);

        Console.WriteLine("Signaing worker...");
        _starter.Set();

        Console.WriteLine("Main. Finish work. Press 'Enter' to quit...");
        Console.ReadLine();
        reg.Unregister(waitObject: _starter);
    }

    private static void Go(object data, bool timeout)
    {
        Console.WriteLine($"Started thread. Passed data: '{data}', timeout: {timeout}");
        Console.WriteLine($"Finish thread");
    }
}
