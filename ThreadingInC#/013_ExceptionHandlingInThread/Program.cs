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
        try
        {
            throw null!;    // Throw NullReferenceException
        }
        catch (Exception ex)
        {
            Console.WriteLine("{0}\nHandle exception in thread", ex.Message);
        }
    }
}
