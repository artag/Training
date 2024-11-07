using System.Threading;
public class Program
{
    public static void Main()
    {
        var foo = new Foo();
        new Thread(() => foo.A()).Start();
        new Thread(() => foo.B()).Start();
    }
}

public class Foo
{
    private int _answer1, _answer2, _answer3;
    private bool _complete;

    public void A()
    {
        _answer1 = 1;
        _answer2 = 2;
        _answer3 = 3;
        Thread.MemoryBarrier();         // Barrier 1
        _complete = true;
        Thread.MemoryBarrier();         // Barrier 2
    }

    public void B()
    {
        Thread.MemoryBarrier();         // Barrier 3
        if (_complete)
        {
            Thread.MemoryBarrier();     // Barrier 4
            Console.WriteLine(_answer1 + _answer2 + _answer3);
        }
    }
}
