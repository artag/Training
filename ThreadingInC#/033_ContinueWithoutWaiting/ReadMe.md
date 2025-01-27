```csharp
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

        Console.WriteLine($"{Now()}: Main. Start sleep.");
        Thread.Sleep(3000);
        Console.WriteLine($"{Now()}: Main. Finish sleep.");

        Console.WriteLine($"{Now()}: Main. Call Set()");
        _wh.Set();

        Console.WriteLine($"{Now()}: Main. Finish. Press 'Enter' to quit.");
        Console.ReadLine();
    }

    private static void GoWithWait(int number)
    {
        Console.WriteLine($"{Now()}: WaitOne. Thread {number} start.");
        MethodWithWait(number);
        Console.WriteLine($"{Now()}: WaitOne. Thread {number} finish.");
    }

    private static void MethodWithWait(int number)
    {
        Console.WriteLine($"    {Now()}: WaitOne. AppServerMethod {number} start.");
        _wh.WaitOne();
        Console.WriteLine($"    {Now()}: WaitOne. AppServerMethod {number} finish.");
    }

    private static void GoNoWait(int number)
    {
        Console.WriteLine($"{Now()}: RegisterWait. Thread {number} start.");
        MethodNoWait(number);
        Console.WriteLine($"{Now()}: RegisterWait. Thread {number} finish.");
    }

    private static void MethodNoWait(int number)
    {
        Console.WriteLine($"    {Now()}: RegisterWait. AppServerMethod {number} start.");
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
        Console.WriteLine($"    {Now()}: RegisterWait. AppServerMethod {number} finish.");
    }

    private static string Now() =>
        DateTime.Now.ToString("ss.fff");
}
```

Вывод:

```text
10.012: Main. Start sleep.
10.014: WaitOne. Thread 1 start.
10.014: RegisterWait. Thread 2 start.
    10.072: WaitOne. AppServerMethod 1 start.
10.015: RegisterWait. Thread 3 start.
    10.072: RegisterWait. AppServerMethod 2 start.
10.015: WaitOne. Thread 0 start.
    10.072: RegisterWait. AppServerMethod 3 start.
    10.074: WaitOne. AppServerMethod 0 start.
10.078: RegisterWait. Thread 3 finish.
10.078: RegisterWait. Thread 2 finish.
13.071: Main. Finish sleep.
13.071: Main. Call Set()
13.072: Main. Finish. Press 'Enter' to quit.
    13.072: WaitOne. AppServerMethod 1 finish.
13.072: WaitOne. Thread 1 finish.
    13.072: WaitOne. AppServerMethod 0 finish.
13.072: WaitOne. Thread 0 finish.
    13.074: RegisterWait. AppServerMethod 3 finish.
    13.074: RegisterWait. AppServerMethod 2 finish.
```
