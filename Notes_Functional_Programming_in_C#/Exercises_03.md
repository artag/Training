1. Write a generic function that takes a `string` and parses it as a value of an `enum`.
It should be usable as follows:

```text
Enum.Parse<DayOfWeek>("Friday")    // => Some(DayOfWeek.Friday)
Enum.Parse<DayOfWeek>("Freeday")   // => None
```

```csharp
public static Option<T> Parse<T>(this string day) where T : struct =>
    System.Enum.TryParse<T>(day, out var result)
        ? Some(result)
        : None;
```

2. Write a Lookup function that will take an `IEnumerable` and a predicate, and
return the first element in the `IEnumerable` that matches the predicate, or `None`
if no matching element is found. Write it's signature in arrow notation:

```text
bool isOdd(int i) => i % 2 == 1;
new List<int>().Lookup(isOdd) // => None
new List<int> { 1 }.Lookup(isOdd) // => Some(1)
```

```csharp
public static Option<T> MyLookup<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
{
    foreach (var item in collection)
        if (predicate(item))
            return Some(item);

    return None;
}
```

3. Write a type Email that wraps an underlying string, enforcing that it's in a valid
 format. Ensure that you include the following:
- A smart constructor.
- Implicit conversion to string, so that it can easily be used with the typical API
for sending emails.

```csharp
public class Email
{
    static readonly Regex _regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

    private readonly string _value;

    private Email(string email) =>
        _value = email;

    public static implicit operator string(Email email) =>
        email._value;

    public static Option<Email> Create(string email) =>
        _regex.IsMatch(email)
            ? Some(new Email(email))
            : None;
}
```

5. Write implementations for the methods in the `AppConfig` class
below. (For both methods, a reasonable one-line method body is possible.
Assume settings are of type string, numeric or date.) Can this implementation help you
to test code that relies on settings in a `.config` file?

```csharp
public class AppConfig
{
    NameValueCollection _source;

    //public AppConfig() : this(ConfigurationManager.AppSettings) { }

    public AppConfig(NameValueCollection source)
    {
        _source = source;
    }

    public Option<T> Get<T>(string key) =>
        source[key] == null
            ? None
            : Some((T)Convert.ChangeType(source[key], typeof(T)));

    public T Get<T>(string key, T defaultValue) =>
        Get<T>(key).Match(
            () => defaultValue,
            (value) => value);
    }
```
