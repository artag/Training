public class Program
{
    static void Main()
    {
        // For .NET 8, on BeginInvoke
        // System.PlatformNotSupportedException: Operation is not supported on this platform.

        Func<string, int> method = GetStringLength;
        var cookie = method.BeginInvoke(arg: "test", callback: null, @object: null);

        Console.WriteLine("Main. Do some work...");

        var result = method.EndInvoke(cookie);
        Console.WriteLine("String length is: " + result);
    }

    private static int GetStringLength(string value)
    {
        return value.Length;
    }
}
