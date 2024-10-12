var tt = new ThreadTest();
new Thread(tt.Go).Start();
tt.Go();

public class ThreadTest
{
    bool _done;

    public void Go()
    {
        if (_done)
            return;

        _done = true;
        Console.WriteLine("Done");
    }
}
