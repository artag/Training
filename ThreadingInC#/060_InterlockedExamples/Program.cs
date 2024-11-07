public class Program
{
    public static void Main()
    {
        var example = new InterlockedExample();
        example.Run();
    }
}

public class InterlockedExample
{
    private long _sum;

    public void Run()
    {
        // Simple increment/decrement operations;
        Interlocked.Increment(ref _sum);                    // 1
        Interlocked.Decrement(ref _sum);                    // 0

        // Add/substract a value;
        Interlocked.Add(ref _sum, 3);                       // 3

        // Read a 64-bit field
        Console.WriteLine(Interlocked.Read(ref _sum));      // 3

        // Write a 64-bit field while reading previous value.
        // (This prints "3" while updating _sum to 10)
        Console.WriteLine(Interlocked.Exchange(
            location1: ref _sum,
            value: 10));                                    // 10

        // Update a field only if it matches a certain value (10).
        // (This prints "10" while updating _sum to 123)
        Console.WriteLine(Interlocked.CompareExchange(
            location1: ref _sum,
            value: 123,
            comparand: 10));                                // 123

        Console.WriteLine(_sum);        // Print "123"
    }
}
