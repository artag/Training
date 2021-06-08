# Maybe

## Extensions

ToResult:

```csharp
Result<TI, TE> ToResult<TI, TE>(this Maybe<TI> maybe, TE error)
{
    if (maybe.HasNoValue)
        return Result.Failure<TI, TE>(error);

    return Result.Success<TI, TE>(maybe.Value);
}
```

Unwrap:

```csharp
TI Unwrap<TI>(this Maybe<TI> maybe, TI defaultValue = default(TI))
{
    return maybe.Unwrap(x => x, defaultValue);
}

TO Unwrap<TI, TO>(this Maybe<TI> maybe, Func<TI, TO> selector, TO defaultValue = default(TO))
{
    if (maybe.HasValue)
        return selector(maybe.Value);

    return defaultValue;
}
```

ToList:

```csharp
List<T> ToList<T>(this Maybe<T> maybe)
{
    return maybe.Unwrap(value => new List<T> {value}, new List<T>());
}
```

Where:

```csharp
Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
{
    if (maybe.HasNoValue)
        return Maybe<T>.None;

    if (predicate(maybe.Value))
        return maybe;

    return Maybe<T>.None;
}
```

Select:

```csharp
Maybe<TO> Select<TI, TO>(this Maybe<TI> maybe, Func<TI, TO> selector)
{
    return maybe.Map(selector);
}
```

SelectMany:

```csharp
Maybe<TO> SelectMany<TI, TO>(this Maybe<TI> maybe, Func<TI, Maybe<TO>> selector)
{
    return maybe.Bind(selector);
}

Maybe<V> SelectMany<T, U, V>(this Maybe<T> maybe,
    Func<T, Maybe<U>> selector, Func<T, U, V> project)
{
    return maybe.Unwrap(
        x => selector(x).Unwrap(u => project(x, u), Maybe<V>.None),
        Maybe<V>.None);
}
```

Map:

```csharp
Maybe<TO> Map<TI, TO>(this Maybe<TI> maybe, Func<TI, TO> selector)
{
    if (maybe.HasNoValue)
        return Maybe<TO>.None;

    return selector(maybe.Value);
}
```

Bind:

```csharp
Maybe<TO> Bind<TI, TO>(this Maybe<TI> maybe, Func<TI, Maybe<TO>> selector)
{
    if (maybe.HasNoValue)
        return Maybe<TO>.None;

    return selector(maybe.Value);
}
```

Execute:

```csharp
void Execute<T>(this Maybe<T> maybe, Action<T> action)
{
    if (maybe.HasNoValue)
        return;

    action(maybe.Value);
}
```

Match:

```csharp
TO Match<TI, TO>(this Maybe<TI> maybe, Func<TI, TO> Some, Func<TO> None)
{
    return maybe.HasValue
        ? Some(maybe.Value)
        : None();
}

void Match<T>(this Maybe<T> maybe, Action<T> Some, Action None)
{
    if (maybe.HasValue)
        Some(maybe.Value);
    else
        None();
}
```

Choose:

```csharp
IEnumerable<U> Choose<T, U>(this IEnumerable<Maybe<T>> source, Func<T, U> selector)
{
    using (var enumerator = source.GetEnumerator())
    {
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            if (item.HasValue)
            {
                yield return selector(item.Value);
            }
        }
    }
}
```

TryFirst:

```csharp
Maybe<T> TryFirst<T>(this IEnumerable<T> source)
{
    if (source.Any())
        return Maybe<T>.From(source.First());

    return Maybe<T>.None;
}

 Maybe<T> TryFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
{
    var firstOrEmpty = source.Where(predicate).Take(1).ToList();
    if (firstOrEmpty.Any())
        return Maybe<T>.From(firstOrEmpty[0]);

    return Maybe<T>.None;
}
```

TryLast:

```csharp
Maybe<T> TryLast<T>(this IEnumerable<T> source)
{
    if (source.Any())
        return Maybe<T>.From(source.Last());

    return Maybe<T>.None;
}

Maybe<T> TryLast<T>(this IEnumerable<T> source, Func<T, bool> predicate)
{
    var last = source.LastOrDefault(predicate);
    if (last != null)
        return Maybe<T>.From(last);

    return Maybe<T>.None;
}
```

TryFind:

```csharp
Maybe<V> TryFind<K, V>(this IReadOnlyDictionary<K, V> dict, K key)
{
    if (dict.ContainsKey(key))
        return dict[key];

    return Maybe<V>.None;
}
```
