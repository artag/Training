public class Program
{
    public static void Main()
    {
        // for (var i = 0; i < 100; i++)
        // {
            var c = new VolatileBrainTeasers();
            var t1 = new Thread(() => c.Test1());
            var t2 = new Thread(() => c.Test2());
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
            Console.WriteLine();
        // }

        // У меня всегда выводит 'a = 0 b = 1'
        // В книге написано, что возможно 'a = 0 b = 0'
    }
}

public class VolatileBrainTeasers
{
    private volatile int _x = 0, _y = 0;

    public void Test1()
    {
        _x = 1;
        int a = _y;
        Console.Write($"a = {a} ");
    }

    public void Test2()
    {
        _y = 1;
        int b = _x;
        Console.Write($"b = {b} ");
    }
}
