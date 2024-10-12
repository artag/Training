public class Program
{
    static void Main()
    {
        var t1 = new Thread(new ThreadStart(Go));
        t1.Start();

        var t2 = new Thread(() => Console.WriteLine("Hello 2!"));
        t2.Start();

        Go();
    }

    private static void Go()
    {
        Console.WriteLine("Hello 1!");
    }
}
