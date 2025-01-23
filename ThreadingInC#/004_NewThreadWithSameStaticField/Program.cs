class Program
{
    public static bool done;

    static void Main()
    {
        new Thread(Go).Start();
        Go();
    }

    private static void Go()
    {
        if (done)
            return;

        // done = true;
        Console.WriteLine("Done");
        done = true;
    }
}
