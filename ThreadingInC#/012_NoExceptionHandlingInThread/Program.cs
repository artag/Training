public class Program
{
    static void Main()
    {
        try
        {
            new Thread(Go).Start();
        }
        catch (Exception)
        {
            Console.WriteLine("We'll never get here!");
        }
    }

    private static void Go()
    {
        throw null!;    // Throw NullReferenceException
    }
}
