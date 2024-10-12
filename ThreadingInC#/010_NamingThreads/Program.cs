public class Program
{
    static void Main()
    {
        Thread.CurrentThread.Name = "main";
        var worker = new Thread(Go);
        worker.Name = "worker";
        worker.Start();
        Go();
    }

    private static void Go()
    {
        Console.WriteLine("Hello from " + Thread.CurrentThread.Name);
    }
}
