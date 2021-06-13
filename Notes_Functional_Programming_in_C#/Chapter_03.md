# Chapter 3

## Designing function signatures and types

## 3.1 Function signature design

### 3.1.1 Arrow notation

Hindley-Milner type signatures in functional languages (F#, Haskell):
```text
f : int -> string
```

"f has type of int to string" - (у функции f тип int в строку)

"f takes an int and yields a string" - (f принимает значение int и выдает string)

In C#: `Func<int, string>`

Function signature |C# type              |Example
-------------------|---------------------|-------------------------------------
int -> string      | Func<int, string>   | (int i) => i.ToString()
() -> string       | Func<string>        | () => "hello"
int -> ()          | Action<int>         | (int i) => WriteLine($"gimme {i}")
() -> ()           | Action              | () => WriteLine("Hello World!")
(int, int) -> int  | Func<int, int, int> | (int a, int b) => a + b    // (int a, int b) - tuple

For HOF's code:

```csharp
public static R Connect<R>(string connStr, Func<IDbConnection, R> func) =>
    Using(
        new SqlConnection(connStr),
        conn => { conn.Open(); return func(conn); });
```

HOF's signature:

```text
(string, (IDbConnection -> R)) -> R
```

C# type: `Func<string, Func<IDbConnection, R>, R>`

### 3.1.2 How informative is a signature?

Signature of `Enumerable.Where`:

```text
(IEnumerable<T>, (T -> bool)) -> IEnumerable<T>
```

Signature of `Enumerable.Zip` (`Zip` - the operation of pairing two parallel lists):

```text
(IEnumerable<A>, IEnumerable<B>, ((A, B) -> C)) -> IEnumerable<C>
```

Некоторые сигнатуры гораздо информативнее других. При разработке API желательно делать
сигнатуры функций как можно более выразительными. Это облегчит использование API и
добавит надежности программам.

## 3.2 Capturing data with data objects

To represent data, we use *data objects*: objects that contain data, but no logic.
These are also called *"anemic"* objects.

In FP (unlike OOP) it's natural to draw a separation between logic and data:

* Logic is encoded in functions.
* Data is captured with data objects, which are used as inputs and outputs to these functions.

Example:

```csharp
// The risk profile
enum Risk { Low, Medium, High }

Risk CalculateRiskProfile(int age) =>
    (age < 60) ? Risk.Low : Risk.Medium;

CalculateRiskProfile(30)        // => Low
CalculateRiskProfile(70)        // => Medium
CalculateRiskProfile("Hello")   // => compiler error: cannot convert from 'string' to 'int'
```

### 3.2.1 Primitive types are often not specific enough

Можно ли улучшить предыдущий код и сделать более явным входное значение?

Ведь можно ввести отрицательное или слишком большое число в качестве age:

```csharp
CalculateRiskProfile(-1000) // => Low
CalculateRiskProfile(10000) // => Medium
```

Стандартное решение в стиле ООП. Ввести валидацию с исключением:

```csharp
Risk CalculateRiskProfile(int age)
{
    if (age < 0 || 120 <= age)
        throw new ArgumentException($"{age} is not a valid age");
    return (age < 60) ? Risk.Low : Risk.Medium;
}

CalculateRiskProfile(10000); // => runtime error: 10000 is not a valid age
```

Недостатки такого подхода:

* You'll have to write additional unit tests for the cases in which validation fails.

* In other areas of the application where an age is expected, so you're probably going
to need the same validation in those places. This will cause some duplication.

* Separation of concerns has been broken. `CalculateRiskProfile` не должна делать валидацию
входных значений.

Можно ли сделать лучше?

### 3.2.2 Constraining inputs with custom types

Implement `Age` - a custom type that can only be instantiated with a valid value for an age:

```csharp
public class Age
{
    // The internal representation is kept private.
    private int Value { get; }

    public Age(int value)
    {
        if (!IsValid(value))
            throw new ArgumentException($"{value} is not a valid age");
        Value = value;
    }

    private static bool IsValid(int age) =>
        0 <= age && age < 120;

    // Logic for comparing an Age with another Age.
    public static bool operator <(Age l, Age r) =>
        l.Value < r.Value;
    public static bool operator >(Age l, Age r) =>
        l.Value > r.Value;

    // For readability, makes it possible to compare an Age with an int;
    // the int will first be converted into an Age.
    public static bool operator <(Age l, int r) =>
        l < new Age(r);
    public static bool operator >(Age l, int r) =>
        l > new Age(r);
}
```

Rewrite previous function:

```csharp
Risk CalculateRiskProfile(Age age) =>
    (age < 60) ? Risk.Low : Risk.Medium;
```

Плюсы такого подхода:

* Используются только правильные значения для возраста

* Валидация возраста делается только в одном месте.

Недостаток:

* В конструкторе `Age` все еще выбрасывается исключение, если входное значение неправильное.
(Далее этот недостаток будет решен).

### 3.2.3 Writing "honest" functions

*Honest* (честная) function - функция, результат выполнения которой всегда соответствует ее
сигнатуре.

Например такой вариант функции является honest:

```csharp
Risk CalculateRiskProfile(Age age) =>
    (age < 60) ? Risk.Low : Risk.Medium;
```

Ее сигнатура `Age -> Risk` говорит: "Give me an `Age` and I will give you back a `Risk`."
И больше никаких других возможных вариантов выполнения.

А такой вариант *dishonest*:

```csharp
Risk CalculateRiskProfile(int age)
{
    if (age < 0 || 120 <= age)
        throw new ArgumentException($"{age} is not a valid age");
    return (age < 60) ? Risk.Low : Risk.Medium;
}
```

Ее сигнатура `int -> Risk` говорит: "Give me an `int` (any of the 2^32 possible values for 
`int`) and I'll return a `Risk`."

Но сигнатура не всегда соблюдается - при некоторых `int` будет выбрасываться `ArgumentException`.

Говорят что такая функция *dishonest* (нечестная).
Что ее сигнатура должна говорить в реальности: "Give me an `int`, and I may return a `Risk`,
or I may throw an exception instead."

**Резюме**: function is *honest* if its behavior can be predicted by its signature: it
returns a value of the declared type; no throwing exceptions, and no `null` return values.

### 3.2.4 Composing values with tuples and objects

Введем больше данных для более корректного CalculateRiskProfile:

```csharp
enum Gender { Female, Male }

Risk CalculateRiskProfile(Age age, Gender gender)
{
    var threshold = (gender == Gender.Female) ? 62 : 60;
    return (age < threshold) ? Risk.Low : Risk.Medium;
}
```

Сигнатура: `(Age, Gender) -> Risk`

Всего 2 * 120 = 240 возможных входных комбинаций.

Аналогичное число комбинаций будет, если вместо `Age` и `Gender` задать `HealthData`:

```csharp
class HealthData
{
    public Age Age;
    public Gender Gender;
}
```

**Резюме**: рекомендуется описывать сущности таким образом, чтобы обеспечить точный контроль над
диапазоном входных данных, которые ваши функции должны будут обрабатывать.
Точное/конечное число возможных входных комбинаций вносит ясность. Как только у вас есть контроль
над этими простыми значениями, их легко объединить в более сложные объекты данных.

## 3.3 Modeling the absence of data with Unit
