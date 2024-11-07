public class Program
{
    public static void Main()
    {
        for (var i = 0; i < 1000; i++)
        {
            var foo = new Foo();
            new Thread(() => foo.A()).Start();
            new Thread(() => foo.B()).Start();
        }

        // Возможно срабатывание методов в порядке:
        // - A, B
        // - B, A
        // Но у меня всегда получался порядок A, B.
    }
}

public class Foo
{
    private int _answer;
    private bool _complete;

    public void A()
    {
        _answer = 123;
        _complete = true;
    }

    public void B()
    {
        if (_complete)
            Console.Write(_answer + " ");
        else
            Console.WriteLine("Answer not set");
    }
}
