```csharp
public class Program
{
    public static void Main()
    {
        var club = new Club();
        for (var i = 0; i < 10; i++)
        {
            new Thread(club.Enter!).Start(i);
        }
    }
}

public class Club
{
    private static readonly SemaphoreSlim _sem =
        new SemaphoreSlim(initialCount: 3);     // Capacity of 3

    public void Enter(object id)
    {
        Console.WriteLine(id + " wants to enter");

        _sem.Wait();

        Console.WriteLine(id + " is in!");
        var rnd = new Random().Next(1, 6);
        Thread.Sleep(TimeSpan.FromSeconds(rnd));
        Console.WriteLine(id + " is leaving!");

        _sem.Release();
    }
}
```

Вывод:

```text
0 wants to enter
3 wants to enter
4 wants to enter
1 wants to enter
7 wants to enter
6 wants to enter
5 wants to enter
2 wants to enter
8 wants to enter
9 wants to enter
9 is in!
1 is in!
6 is in!
6 is leaving!
8 is in!
8 is leaving!
7 is in!
7 is leaving!
5 is in!
9 is leaving!
1 is leaving!
3 is in!
0 is in!
5 is leaving!
2 is in!
3 is leaving!
0 is leaving!
4 is in!
4 is leaving!
2 is leaving!
```
