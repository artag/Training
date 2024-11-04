public class Program
{
    // Not recommended use Thread as timer

    public static void Main()
    {
        var enabled = true;
        ThreadPool.QueueUserWorkItem(data =>
        {
            while (enabled)
            {
                DoSomeAction();
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine("Timer stopped");
        });

        Console.WriteLine("Press 'Enter' to quit...");
        Console.ReadLine();

        enabled = false;
        Console.WriteLine("Done");
    }

    private static void DoSomeAction()
    {
        Console.WriteLine($"{DateTime.Now.ToString("ss, fff")}: tick");
    }
}
