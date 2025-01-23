public class Program
{
    static void Main()
    {
        ThreadPool.QueueUserWorkItem(Go);
        ThreadPool.QueueUserWorkItem(Go, 123);

        // true, чтобы предпочитать ставить рабочий элемент в очередь близко к текущему потоку;
        // false, чтобы предпочитать ставить рабочий элемент в общую очередь пула потоков.
        ThreadPool.QueueUserWorkItem<string>(GoParametrized, "hello world", preferLocal: false);

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
