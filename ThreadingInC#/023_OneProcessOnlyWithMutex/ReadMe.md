```csharp
public class Program
{
    public static void Main(string[] args)
    {
        // Для блокировки из разных терминалов надо добавить префикс "Global\".
        using var mutex = new Mutex(initiallyOwned: false, name: @"Global\program_lock");
        if (!mutex.WaitOne(timeout: TimeSpan.FromSeconds(3), exitContext: false))
        {
            Console.WriteLine("Another instance of the app is running. Bye!");
            return;
        }

        Run();
    }

    private static void Run()
    {
        Console.WriteLine("Running. Press 'Enter' to exit");
        Console.ReadLine();
    }
}
```

Вывод. Требуется одновременный запуск в обоих терминалах.

В первом терминале:

```text
Running. Press 'Enter' to exit
```

Во втором терминале:

```text
Another instance of the app is running. Bye!
```
