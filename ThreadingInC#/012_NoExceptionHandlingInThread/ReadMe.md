```csharp
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
        throw null!;    // Throw NullReferenceException
    }
}
```

Вывод:

```text
Unhandled exception. System.NullReferenceException: Object reference not set to an instance of an object.
   at Program.Go() in /home/agart/Projects/Training/ThreadingInC#/012_NoExceptionHandlingInThread/Program.cs:line 17
```
