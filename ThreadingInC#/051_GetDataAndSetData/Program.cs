public class Program
{
    public static void Main()
    {
        var t = new Test();
        Thread.CurrentThread.Name = "Main";

        var t1 = new Thread(() =>
        {
            t.DisplaySecurityLevel();
            t.SecurityLevel = 33;
            t.DisplaySecurityLevel();
        });
        t1.Name = "Thread 1";

        var t2 = new Thread(() =>
        {
            t.DisplaySecurityLevel();
            t.SecurityLevel = 42;
            t.DisplaySecurityLevel();
        });
        t2.Name = "Thread 2";

        t1.Start();

        t.DisplaySecurityLevel();
        t.SecurityLevel = 256;
        t.DisplaySecurityLevel();

        t2.Start();

        t1.Join();
        t2.Join();
        Console.WriteLine("Done");

        // Display:
        // Main. Security level 0
        // Thread 1. Security level 0
        // Main. Security level 256
        // Thread 1. Security level 33
        // Thread 2. Security level 0
        // Thread 2. Security level 42
        // Done
    }
}

public class Test
{
    // The same LocalDataStoreSlot object can be used across all threads.
    private LocalDataStoreSlot _slot = Thread.GetNamedDataSlot(name: "securityLevel");

    public int SecurityLevel
    {
        get
        {
            var data = Thread.GetData(_slot);
            return data == null     // null == unitialized
                ? 0
                : (int)data;
        }
        set
        {
            Thread.SetData(_slot, value);
        }
    }

    public void DisplaySecurityLevel() =>
        Console.WriteLine(
            $"{Thread.CurrentThread.Name}. Security level {SecurityLevel}");
}
