public class Program
{
    static void Main()
    {
        // Bad
        for (var i = 0; i < 10; i++)
            new Thread(() => Console.Write(i)).Start();

        Console.WriteLine();

        // Good
        for (var i = 0; i < 10; i++)
        {
            int temp = i;
            new Thread(() => Console.Write(temp)).Start();
        }

        Console.WriteLine();

        var text = "t1";
        var t1 = new Thread(() => Console.WriteLine("From t1: " + text));
        text = "t2";
        var t2 = new Thread(() => Console.WriteLine("From t2: " + text));

        t1.Start();     // Print: t2
        t2.Start();     // Print: t2
    }
}
