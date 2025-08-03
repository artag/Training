```csharp
public class Program
{
    public static void Main()
    {
        var example = new Example();
        var t1 = new Thread(() => example.ChangeNumber(n => n * 3));
        var t2 = new Thread(() => example.ChangeNumber(n => n + 6));

        t1.Name = "Thread 1";
        t2.Name = "Thread 2";

        Thread.CurrentThread.Name = "Main";
        example.ChangeNumber(n => n + 1);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
        Console.WriteLine("Done");
    }
}

public class Example
{
    [ThreadStatic]
    public static int number;

    public void ChangeNumber(Func <int, int> func)
    {
        number = 1;
        for (var i = 0; i < 3; i++)
        {
            number = func(number);
            Console.WriteLine($"{Thread.CurrentThread.Name}: number is {number}");
        }
    }
}

```

```text
Main: number is 2
Main: number is 3
Main: number is 4
Thread 1: number is 3
Thread 1: number is 9
Thread 1: number is 27
Thread 2: number is 7
Thread 2: number is 13
Thread 2: number is 19
Done
```
