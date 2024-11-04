public class Program
{
    public static void Main()
    {
        Console.WriteLine("First interval 2000ms, subsequent intervals = 1000ms");
        var timer = new System.Threading.Timer(
            callback: Tick!,
            state: "tick...",
            dueTime: 2000,
            period: 1000);

        Console.WriteLine("Press 'Enter' to set period to 500ms");
        Console.ReadLine();
        timer.Change(dueTime: 500, period: 500);

        Console.WriteLine("Press 'Enter' to set one-shot timer work mode");
        Console.ReadLine();
        timer.Change(dueTime: 500, period: Timeout.Infinite);

        Console.WriteLine("Press 'Enter' to quit");
        Console.ReadLine();
        timer.Dispose();
    }

    private static void Tick(object data)
    {
        // This runs on a pooled thread
        Console.WriteLine($"{DateTime.Now.ToString("ss, fff")}: {data}");    // Writes "tick..."
    }
}
