class Program
{
    static void Main()
    {
        var t = new Thread(Go);
        t.Start();
        t.Join();
        Console.WriteLine("Thread t has ended!");
    }

    private static void Go()
    {
        for (var i = 0; i < 100; i++)
            Console.Write("y");
    }
}
