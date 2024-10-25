public class Program
{
    private static ManualResetEvent _wh = new ManualResetEvent(initialState: false);

    public static void Main()
    {
        for (var i = 0; i < 2; i++)
        {
            var num = i;
            new Thread(() => GoWithWait(num)).Start();
        }
        for (var i = 2; i < 4; i++)
        {
            var num = i;
            new Thread(() => GoNoWait(num)).Start();
        }

        Console.WriteLine("Main thread start.");
        Thread.Sleep(3000);

        Console.WriteLine("Main thread call Set()");
        _wh.Set();

        Console.WriteLine("Main thread finish. Press 'Enter' to quit.");
        Console.ReadLine();
    }

    private static void GoWithWait(int number)
    {
        Console.WriteLine($"Thread {number} start.");
        MethodWithWait(number);
        Console.WriteLine($"Thread {number} finish.");
    }

    private static void MethodWithWait(int number)
    {
        Console.WriteLine($"AppServerMethod {number} start.");
        _wh.WaitOne();
        Console.WriteLine($"AppServerMethod {number} finish.");
    }

    private static void GoNoWait(int number)
    {
        Console.WriteLine($"Thread {number} start.");
        MethodNoWait(number);
        Console.WriteLine($"Thread {number} finish.");
    }

    private static void MethodNoWait(int number)
    {
        Console.WriteLine($"AppServerMethod {number} start.");
        var reg = ThreadPool.RegisterWaitForSingleObject(
            waitObject: _wh,
            callBack: Resume!,
            state: number,
            millisecondsTimeOutInterval: -1,
            executeOnlyOnce: true);
    }

    private static void Resume(object data, bool timeOut)
    {
        var number = (int)data;
        Console.WriteLine($"AppServerMethod {number} finish.");
    }
}
