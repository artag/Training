# Exercises 04

1. Implement `Map` for `ISet<T>` and `IDictionary<K, T>`.
(Tip: start by writing down the signature in arrow notation.)

// Map : ISet<T> -> (T -> R) -> ISet<R>
```csharp
static ISet<R> Map<T, R>(this ISet<T> ts, Func<T, R> f)
{
    var rs = new HashSet<R>();
    foreach (var item in ts)
        rs.Add(f(item));
    return rs;
}
```

// Map : IDictionary<K, T> -> (T -> R) -> IDictionary<K, R>
```csharp
static IDictionary<K, R> Map<K, T, R>(this IDictionary<K, T> dict, Func<T, R> f)
{
    var rs = new Dictionary<K, R>();
    foreach (var pair in dict)
        rs[pair.Key] = f(pair.Value);
    return rs;
}
```

2. Implement `Map` for `Option` and `IEnumerable` in terms of `Bind` and `Return`.

// Map : Option<T> -> (T -> R) -> Option<R>
```csharp
public static Option<R> Map<T, R>(this Option<T> opt, Func<T, R> f) =>
    opt.Bind(t => Some(f(t)));
```

// Map : IEnumerable<T> -> (T -> R) -> IEnumerable<R>
```csharp
public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f) =>
    ts.Bind(t => Some(f(t)));
```

3. Use `Bind` and an `Option`-returning `Lookup` function (such as the one we defined
in chapter 3) to implement `GetWorkPermit`, shown below. 

Дано:
```csharp
public class Employee
{
    public string Id { get; set; }
    public Option<WorkPermit> WorkPermit { get; set; }

    public DateTime JoinedOn { get; }
    public Option<DateTime> LeftOn { get; }
}

public struct WorkPermit
{
    public string Number { get; set; }
    public DateTime Expiry { get; set; }
}
```

```csharp
public static Option<WorkPermit> GetWorkPermit(
    Dictionary<string, Employee> employees, string employeeId) =>
        employees
            .Lookup(employeeId)
            .Bind(e => e.WorkPermit);

private static Option<T> Lookup<K, T>(this IDictionary<K, T> dict, K key) =>
    dict.TryGetValue(key, out var value)
        ? Some(value)
        : None;

```

Then enrich the implementation so that `GetWorkPermit`
returns `None` if the work permit has expired.

```csharp
public static Option<WorkPermit> GetWorkPermitExt(
    Dictionary<string, Employee> employees, string employeeId) =>
        employees
            .Lookup(employeeId)
            .Bind(e => e.WorkPermit)
            .Where(p => HasExpired(p));

private static Func<WorkPermit, bool> HasExpired =>
    permit => permit.Expiry < DateTime.Now;
```

Или так:

```csharp
public static Option<WorkPermit> GetWorkPermitExt(
    Dictionary<string, Employee> employees, string employeeId) =>
            employees
                .Lookup(employeeId)
                .Bind(e => e.WorkPermit)
                .Where(HasExpired);        // .Where(p => HasExpired(p));

private static bool HasExpired(this WorkPermit permit) =>
    permit.Expiry < DateTime.Now;
```

4. Use `Bind` to implement `AverageYearsWorkedAtTheCompany`, shown below (only employees
who have left should be included).

```csharp
public static double AverageYearsWorkedAtTheCompany(List<Employee> employees) =>
    employees
        .Bind(e => e.LeftOn.Map(leftOn => YearsBetween(e.JoinedOn, leftOn)))
        .Average();

private static double YearsBetween(DateTime start, DateTime end) =>
    (end - start).Days / 365d;
```