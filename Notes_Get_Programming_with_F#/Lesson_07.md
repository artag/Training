# Lesson 7. Expressions and statements

## Comparing statements (инструкции) and expressions (выражения)

C# - statement-based language.

F# - use expressions as the default way of working.

### Statements and expressions compared

| -          | Returns something? | Has side-effects?
|------------|--------------------|------------------
| Statements | Never              | Always
| Expressions| Always             | Rarely

### Пример использования statements in C#

```csharp
public void DescribeAge(int age)
{
    string ageDescription = null;    // Initializes a mutable variable to a default
    var greeting = "Hello";          // Creates a mutable variable to use later

    if (age < 18)
        ageDescription = "Child!";   // First if branch
    else if (age < 65)
        greeting = "Adult!";         // Second if branch

    Console.WriteLine($"{greeting}! You are a '{ageDescription}'.");
}
```

Ошибки:
* No handler for the case when `age >= 65`.
* Значение `ageDescription` может так и остаться установленным в `null` и компилятор к этому
состоянию нормально отнесется.
  * Во второй ветке значение присваивается `greeting` вместо `ageDescription`.
  * Вероятен `NullReferenceException` при попытке вывода на консоль.

### Пример использования expressions in C#

```csharp
// Expression with signature int -> string
private static string GetText(int age) {
    if (age < 18) return "Child!";
    else if (age < 65) return "Adult!";
    else return "OAP!";
}

public void DescribeAge(int age) {
    // Callsite to function
    var ageDescription = GetText(age);
    var greeting = "Hello";
    Console.WriteLine($"{greeting}! You are a '{ageDescription}'.");
}
```

Преимущества (по сравнению с предыдущим подходом):
* Два метода, каждый имеет *single responsibility*.
* Улучшенная читаемость кода.
* You can no longer omit (пропускать) the `else` case when generating the description.
Если пропустить, то компилятор покажет ошибку: "not all code paths return a value".
* Присвоение значения `ageDescription` выполняется только в одном месте - больше
не получится случайно присвоить значение другой переменной.
* Не нужно предварительно иниуиализировать `ageDescription` значением `null`.
* Immutable значения у `ageDescription` и `greeting`.

## Using expressions in F#

* Virtually everything in F# is an expression.
* F# has no notion of a `void` function. Every function *must* return something.
* All program-flow branching mechanisms are expressions.
* All values are expressions.

Т.к. в F# все функции возвращают значения, то не требуется использовать ключевое слово `return`.
Последнее выражение в функции является возвращаемым значением.

### Пример использования expressions in F#. Encouraging composability

```fsharp
open System
let describeAge age =
    let ageDescription =                // Value binding
        if age < 18 then "Child!"       // if/else expression branches
        elif age < 65 then "Adult!"
        else "OAP!"

let greeting = "Hello"
Console.WriteLine("{0}! You are a '{1}'.", greeting, ageDescription)
```

Сравнение с подобным кодом на C#:
* `if/then` block acts more like a function.
* `ageDescription` задается в этой же функции.

Еще одно преимущество от использования expressions это *encourage composability*
(поощрение композиционности). Можно выделить ageDescription в отдельную функцию и
переиспользовать ее.

### Introducing unit

Вместо `void` используется `unit`.

Можно сказать следующее:

1. Every function returns a value - *even if that value is `unit`*.

2. Every function always takes in at least one input value, *even if that value is `unit`*.

Чем плох `void`? В C# это особый случай и для него требуется отдельный тип. Например
`Task` и `Task<T>`. В F# будет требоваться только `Task<T>`, там где функция ничего не
возвращает будет использоваться `Task<unit>`.

>### unit isn’t quite an object
>Unfortunately, despite unifying the type system, at runtime `unit` doesn't behave quite like
>a proper .NET object. For example, don't try to call `GetHashCode()` or `GetType()` on it,
>because you'll get a null reference exception. A future version of F# will fix this,
>but you can still think of `unit` as a singleton object if it helps you to visualize it.

### Explicitly ignoring the result of an expression

```fsharp
let writeTextToDisk text =                          // Writes text to disk
    let path = System.IO.Path.GetTempFileName()
    System.IO.File.WriteAllText(path, text)
    path

let createManyFiles() =
    writeTextToDisk "The fox jumped over the lazy dog"    // warning from compiler
    writeTextToDisk "The fox jumped over the lazy dog"    // warning from compiler
    writeTextToDisk "The fox jumped over the lazy dog"

// Calls the function
createManyFiles()
```

Для первых двух вызовов функций будет выданы warning, т.к. вызвращаются `string`и и они
не используются.

Одно из рещений - использование `ignore`.
`ignore` takes in a value and discards it, before returning `unit`.

```fsharp
// Using ignore
let createManyFiles() =
    ignore(writeTextToDisk "The fox jumped over the lazy dog")
    ignore(writeTextToDisk "The fox jumped over the lazy dog")
    writeTextToDisk "The fox jumped over the lazy dog"
```

### Forcing statement-based code with unit

Иногда (редко) бывает, что необходимо работать с функциями в виде statements. 

Нужно проигнорировать возвращаемое значение, используя `ignore`.
Например, можно сделать так:

```fsharp
let now = System.DateTime.UtcNow.TimeOfDay.TotalHours
if now < 12.0 then Console.WriteLine "It's morning"         // returns unit
elif now < 18.0 then Console.WriteLine "It's afternoon"     // returns unit
elif now < 20.0 then ignore(5 + 5)                          // (1)
else ()                                                     // (2)

// (1) Ignoring an expression to return unit
// (2) else branch here is optional - explicitly returning unit for the final case
```
