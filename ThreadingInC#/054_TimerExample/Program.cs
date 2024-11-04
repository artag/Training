using System.Timers;

public class Program
{
    public static void Main()
    {
        var tmr = new System.Timers.Timer(); // Doesn't require any args
        tmr.Interval = 1000;
        tmr.Elapsed += tmr_Elapsed;          // Uses an event intead of a delegate
        tmr.Start();            // Start the timer

        Console.WriteLine("Press 'Enter' to stop the timer");
        Console.ReadKey();
        tmr.Stop();             // Stop the timer

        Console.WriteLine("Press 'Enter' to restart the timer");
        Console.ReadKey();
        tmr.Start();            // Restart the timer

        Console.WriteLine("Press 'Enter' to permanently stop the timer");
        Console.ReadKey();
        tmr.Dispose();          // Permanently stop the timer
    }

    private static void tmr_Elapsed(object? sender, ElapsedEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now.ToString("ss, fff")}: tick...");
    }
}
