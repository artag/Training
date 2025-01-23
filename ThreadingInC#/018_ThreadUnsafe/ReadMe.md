```csharp
public class Program
{
    public static void Main()
    {
        var thread = new Thread(ThreadUnsafe.Go);

        thread.Start();
        ThreadUnsafe.Go();

        Console.WriteLine("End");
    }
}

public static class ThreadUnsafe
{
    static int _val1 = 1;
    static int _val2 = 1;

    public static void Go()
    {
        if (_val2 != 0)
            Console.WriteLine(_val1 / _val2);
        _val2 = 0;
    }
}
```

Вывод такой, но потенциально возможна ситуация деления на 0:

```text
1
End
1
```
