```csharp
public class Program
{
    public static void Main()
    {
        var ts = new ThreadSafe();
        new Thread(() => ts.AddItem("T1")).Start();
        new Thread(() => ts.AddItem("T2")).Start();
    }
}

public class ThreadSafe
{
    private readonly List<string> _list = new List<string>();

    public void AddItem(string name)
    {
        lock (_list)
        {
            var str = $"{name}. Item {_list.Count}";
            _list.Add(str);
            Console.WriteLine($"-- Add: {str}");
        }

        string[] items;
        lock (_list)
        {
            items = _list.ToArray();
        }

        foreach (string s in items)
        {
            Console.WriteLine(s);
        }
    }
}
```

Вывод:

```text
-- Add: T1. Item 0
T1. Item 0
-- Add: T2. Item 1
T1. Item 0
T2. Item 1
```
