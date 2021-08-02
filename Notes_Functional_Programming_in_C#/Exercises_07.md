# Exercises 07

1. Partial application with a binary arithmetic function:

1.1. Write a function, `Remainder`, that calculates the remainder of integer division
(and works for negative input values!). Notice how the expected order of
parameters isn't the one that's most likely to be required by partial application
(you're more likely to partially apply the divisor (делитель)).

```csharp
Func<int, int, Option<int>> Remainder = (dividend, divisor) =>
    divisor == 0
        ? None
        : Some(dividend % divisor);
```

Manually enabling partial application:

```csharp
Func<int, Func<int, Option<int>>> RemainderInternal = divisor => dividend =>
    divisor == 0
        ? None
        : Some(dividend % divisor);

Func<int, Option<int>> RemainderDivideBy5 =>
    RemainderInternal(5);

Func<int, Option<int>> RemainderDivideBy0 =>
    RemainderInternal(0);
```

1.2. Write an `ApplyR` function that gives the rightmost parameter to a given
binary function. (Try to do so without looking at the implementation for
`Apply`.) Write the signature of `ApplyR` in arrow notation, both in curried and
non-curried forms.

```csharp
// ApplyR: (((T1, T2) -> R), T2) -> T1 -> R
// ApplyR: (T1 -> T2 -> R) -> T2 -> T1 -> R (curried form)
Func<T1, R> ApplyR<T1, T2, R>(this Func<T1, T2, R> func, T2 t2) =>
    t1 => func(t1, t2);
```

1.3. Use `ApplyR` to create a function that returns the remainder of dividing any
number by 5.

```csharp
Func<int, Option<int>> RemainderDivideBy5_v2 = Remainder.ApplyR(5);
```

1.4. Write an overload of `ApplyR` that gives the rightmost argument to a ternary
function.

```csharp
Func<T1, T2, R> ApplyR<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T3 t3) =>
    (t1, t2) => func(t1, t2, t3);
```

Тест, для тестирования 1.1 - 1.3:

```csharp
[Test]
public void Eight_divide_by_five_Return_some_three()
{
    var expected = Some(3);
    var x = 8;
    var y = 5;      // 8 % 5

    var actual1 = Remainder(x, y);          // Обычный метод
    Assert.AreEqual(expected, actual1);

    var actual2 = RemainderDivideBy5(x);    // Manually enabling partial application
    Assert.AreEqual(expected, actual2);

    var partial3 = Remainder.Apply(x);                  // Apply
    var actual3 = partial3(y);
    Assert.AreEqual(expected, actual3);

    // RemainderDivideBy5_v2 = Remainder.ApplyR(5);
    var actual4 = RemainderDivideBy5_v2(x);             // ApplyR
    Assert.AreEqual(expected, actual4);
}
```
