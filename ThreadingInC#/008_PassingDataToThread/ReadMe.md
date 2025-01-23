```csharp
public class Program
{
    static void Main()
    {
        var t1 = new Thread(() => Print("Hello from t1"));
        t1.Start();

        new Thread(() => {
            Console.WriteLine("I'm running on t2");
            Console.WriteLine("Hello from t2");
        }).Start();

        new Thread(delegate()
        {
            Console.WriteLine("I'm running on t3");
            Console.WriteLine("Hello from t3");
        }).Start();

        var t4 = new Thread(Print2);
        t4.Start("Hello from t4");

        Print("Hello from Main");
    }

    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private static void Print2(object? messageObj)
    {
        var message = (string)messageObj!;
        Console.WriteLine(message);
    }
}
```

Вывод:

```text
I'm running on t2
Hello from t2
Hello from t1
I'm running on t3
Hello from t3
Hello from Main
Hello from t4
```
