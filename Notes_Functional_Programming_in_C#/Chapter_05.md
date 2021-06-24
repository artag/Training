# Chapter 5

## Designing programs with function composition

## 5.1 Function composition

### 5.1.1 Brushing up on function composition

Function composition in math:

```text
h = f * g
h(x) = (f * g)(x) = f(g(x))
```

Example:

```csharp
static string AbbreviateName(Person p) =>
    Abbreviate(p.FirstName) + Abbreviate(p.LastName);

static string Abbreviate(string s) =>
    s.Substring(0, 2).ToLower();

static string AppendDomain(string localPart) =>
    $"{localPart}@manning.com";
```

Defining a function as the composition of two existing functions:

```csharp
// emailFor is the composition of AppendDomain with AbbreviateName.
Func<Person, string> emailFor =
    p => AppendDomain(AbbreviateName(p));
```

Usage:

```csharp
var joe = new Person("Joe", "Bloggs");
var email = emailFor(joe);
// => jobl@manning.com
```

Важно отметить:

1. Можно compose только функции с совпадающими типами: если composing `(f * g)`, то
выход `g` должен быть assignable для входного типа `f`.

2. В композиции функций, функции идут в обратном порядке в котором они выполняются
(справа налево).

### 5.1.2 Method chaining

Предыдущий пример можно модифицировать следующим образом:

```csharp
// Add the this keyword to make it an extension method.
static string AbbreviateName(this Person p) =>
    Abbreviate(p.FirstName) + Abbreviate(p.LastName);

static string AppendDomain(this string localPart) =>
    $"{localPart}@manning.com";
```

Using method chaining syntax to compose functions:

```csharp
var joe = new Person("Joe", "Bloggs");
var email = joe.AbbreviateName().AppendDomain();
// => jobl@manning.com
```

The extension methods appear in the order in which they will be executed.
This significantly improves readability. 

Method chaining is the preferable way of achieving function
composition in C#.

### 5.1.3 Composition in the elevated world

```csharp
Func<Person, string> emailFor =
    p => AppendDomain(AbbreviateName(p));

var opt = Some(new Person("Joe", "Bloggs"));    // Option<Person>

// Maps the composed functions
var a = opt.Map(emailFor)       // opt.Map(p => emailFor(p))

// Maps AbbreviateName and AppendDomain in separate steps
var b = opt.Map(AbbreviateName)
           .Map(AppendDomain);

Assert.AreEqual(a, b);
```

>#### Не совсем понятно
>More generally, if `h = f * g`, then mapping `h` onto a functor should be equivalent to
>mapping `g` over that functor and then mapping `f` over the result. This should hold for
>any functor, and for any pair of functions — it's one of the *functor laws*, so any
>implementation of `Map` should observe (соблюдать) it.

`Map` должен применять функцию к внутренним значениям functor и ничего больше не делать,
чтобы композиция функций сохранялась при работе с функторами так же, как и с обычными значениями.

Прелесть этого в том, что вы можете использовать любую функциональную библиотеку
на любом языке программирования и использовать любой функтор с уверенностью, что рефакторинг,
такой как изменение для `a` и `b` в предыдущем примере, будет безопасным.

## 5.2 Thinking in terms of data flow
