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
