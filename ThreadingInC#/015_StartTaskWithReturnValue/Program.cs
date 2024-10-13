using System.Net;

public class Program
{
    static void Main()
    {
        var task = Task.Factory.StartNew<string>(
            () => DownloadString("http://google.com")
        );

        Console.WriteLine("Do some work in the main thread");

        var result = task.Result ?? string.Empty;
        var str = result.Length > 64 ? result.Substring(0, 64) : result;
        Console.WriteLine("String from the task: '" + str + "...'");
    }

    private static string DownloadString(string uri)
    {
        using var wc = new WebClient();
        return wc.DownloadString(uri);
    }
}
