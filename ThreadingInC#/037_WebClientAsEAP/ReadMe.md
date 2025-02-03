```csharp
using System.Net;

public class Program
{
    public static void Main()
    {
        // Obsolete. Устарел, рекомендуется использовать HttpClient
        var wc = new WebClient();
        wc.DownloadStringCompleted += (sender, args) =>
        {
            if (args.Cancelled)
                Console.WriteLine("Cancelled");
            else if (args.Error != null)
                Console.WriteLine("Exception: " + args.Error.Message);
            else
            {
                Console.WriteLine(args.Result.Length + " chars were downloaded");
                // We could update the UI from here...
            }
        };

        wc.DownloadStringAsync(new Uri("http://google.com"));       // Start download

        Console.WriteLine("Press 'Enter' to cancel download task and quit");
        Console.ReadKey();
    }
}
```

Вывод:

```text
Press 'Enter' to cancel download task and quit
55572 chars were downloaded
```

Вывод, если быстро нажать `Enter` (прерывание загрузки):

```text
Press 'Enter' to cancel download task and quit
```
