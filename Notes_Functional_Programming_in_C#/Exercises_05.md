# Exercises 05

1. Without looking at any code or documentation, write the type of the functions
`OrderBy`, `Take`, and `Average`, which were used to implement `AverageEarningsOfRichestQuartile`.

```csharp
static decimal AverageEarningsOfRichestQuartile(List<Person> population) =>
    population
      .OrderByDescending(p => p.Earnings)
      .Take(population.Count / 4)
      .Select(p => p.Earnings)
      .Average();
```

```text
OrderByDescending : (IEnumerable<T>, (T -> decimal)) -> IEnumerable<T>
Take : (IEnumerable<T>, int) -> IEnumerable<T>
Select : (IEnumerable<T>, (T -> R)) -> IEnumerable<R>
Average : IEnumerable<T> -> T
```

3. Implement a general purpose Compose function that takes two unary functions
and returns the composition of the two.

```csharp
static Func<T1, R> Compose<T1, T2, R>(Func<T1, T2> f, Func<T2, R> g) =>
    x => g(f(x));
```

Usage:
```csharp
static int IntToString(string num) => int.Parse(num);
static bool IsEven(int num) => num % 2 == 0;
static Func<string, bool> StringToBool = Compose<string, int, bool>(IntToString, IsEven);
```

Test:
```csharp
[Test]
[TestCase("2", ExpectedResult = true)]
[TestCase("7", ExpectedResult = false)]
public bool Input_string_Check_IsEven(string num) =>
    StringToBool(num);
```
