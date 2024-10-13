

public class Program
{
    static void Main()
    {
        ThreadPool.QueueUserWorkItem(Go);
        ThreadPool.QueueUserWorkItem(Go, 123);
        ThreadPool.QueueUserWorkItem<string>(GoParametrized, "hello world", false);

        ThreadPool.QueueUserWorkItem(obj =>
        {
            try
            {
                GoWithException(obj);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Main. Catch exception from thread: {0}", ex.Message);
            }
        });

        Console.WriteLine("Press 'Enter' to quit");
        Console.ReadLine();
    }

    private static void Go(object? data)
    {
        Console.WriteLine("From thread 'Go' with data: '{0}'", data);
    }

    private static void GoParametrized(string data)
    {
        Console.WriteLine("From thread 'Go' with parametrized data: '{0}'", data);
    }

    private static void GoWithException(object? state)
    {
        throw new InvalidOperationException($"Exception from {nameof(GoWithException)}");
    }
}
