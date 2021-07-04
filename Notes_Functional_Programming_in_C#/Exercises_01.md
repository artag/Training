# Exercises 01

2. Write a function that negates a given predicate: whenever the given predicate
evaluates to `true`, the resulting function evaluates to `false`, and vice versa.

```csharp
static Func<T, bool> Negate<T>(this Func<T, bool> predicate) =>
    t => !predicate(t);
```

3. Write a method that uses quicksort to sort a `List<int>` (return a new list, rather
than sorting it in place).

```csharp
// Not quicksort but bubble sort.
static IEnumerable<int> BubbleSort(this IEnumerable<int> list)
{
    var localList = list.ToArray();

    for (var i = 0; i < localList.Length; i++)
    {
        var swapped = false;

        for (var j = 0; j < localList.Length - 1 - i; j++)
        {
            if (localList[j] > localList[j + 1])
            {
                var tmp = localList[j];
                localList[j] = localList[j + 1];
                localList[j + 1] = tmp;
                swapped = true;
            }
        }

        if (!swapped)
            return localList;
    }

    return localList;
}
```

4. Generalize the previous implementation to take a `List<T>` , and additionally a
`Comparison<T>` delegate.

```csharp
static IEnumerable<T> BubbleSortWithComparison<T>(
    this IEnumerable<T> list, Comparison<T> comparison)
{
    var localList = list.ToArray();

    for (var i = 0; i < localList.Length; i++)
    {
        var swapped = false;

        for (var j = 0; j < localList.Length - 1 - i; j++)
        {
            if (comparison(localList[j], localList[j + 1]) > 0)
            {
                var tmp = localList[j];
                localList[j] = localList[j + 1];
                localList[j + 1] = tmp;
                swapped = true;
            }
        }

        if (!swapped)
            return localList;
    }

    return localList;
}

// Comparison<int>
static int CompareInt(int x, int y) =>
    x.CompareTo(y);

// Comparison<string>
static int CompareString(string x, string y) =>
    Compare(x, y, StringComparison.Ordinal);

// Usage
list.BubbleSortWithComparison(CompareInt);
list.BubbleSortWithComparison(CompareString);
```

5. In this chapter, you've seen a `Using` function that takes an `IDisposable` and a
function of type `Func<TDisp, R>`. Write an overload of `Using` that takes a
`Func<IDisposable>` as the first parameter, instead of the `IDisposable`.

(This can be used to avoid warnings raised by some code analysis tools about instantiating
an `IDisposable` and not disposing it).

```csharp
public static TR Using<TD, TR>(Func<TD> createDisposable, Func<TD, TR> func)
    where TD : IDisposable
{
    using (var disposable = createDisposable())
    {
        return func(disposable);
    }
}
```
