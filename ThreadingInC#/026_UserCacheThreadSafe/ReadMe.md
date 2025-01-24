```csharp
public class Program
{
    public static void Main()
    {
        var cache = new UserCache();

        new Thread(() => cache.GetUser(42)).Start();
        cache.GetUser(42);
        cache.GetUser(42);
        cache.GetUser(42);
    }
}

public class UserCache
{
    private readonly Dictionary<int, User> _users = new Dictionary<int, User>();

    public User GetUser(int id)
    {
        User? u = null;
        lock (_users)
        {
            if (_users.TryGetValue(id, out u))
            {
                Console.WriteLine($"Read user '{id}' from cache");
                return u;
            }
        }

        u = RetrieveUser(id);
        lock (_users)
        {
            Console.WriteLine($"Save user '{id}' to cache");
            _users[id] = u;
            return u;
        }
    }

    // Some slow operation
    private User RetrieveUser(int id)
    {
        Console.WriteLine($"Get user '{id}' from repository");
        return new User($"Name{id}", id + 10);
    }
}

public record User(
    string Name,
    int Age
);
```

Вывод:

```text
Get user '42' from repository
Save user '42' to cache
Get user '42' from repository
Save user '42' to cache
Read user '42' from cache
Read user '42' from cache
```
