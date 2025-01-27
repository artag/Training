using System.Threading;
using System;
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
