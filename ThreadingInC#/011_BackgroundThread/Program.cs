public class Program
{
    static void Main(string[] args)
    {
        try
        {
            Run(args);
        }
        finally
        {
            Console.WriteLine("Main finally block");
        }
    }

    static void Run(string[] args)
    {
        var worker = new Thread(() =>
        {
            try
            {
                Console.WriteLine("Press 'Enter' to continue");
                Console.ReadLine();
            }
            finally
            {
                Console.WriteLine("Thread finally block");      // No executes when background
            }
        });
        if (args.Length > 0)
            worker.IsBackground = true;

        worker.Start();
    }
}
