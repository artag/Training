# Chapter 7

## Structuring an application with functions

## 7.1 Partial application: supplying arguments piecemeal (по частям)

*Separation of concerns* (разделение ответственности) - it may be best to provide the various
arguments that a function needs at different points in the application lifecycle, and
from different components.

Example. A binary function mapped over a list:

```csharp
// Attach names to string type, thus making the function signatures more meaningful.
using Name = System.String;
using Greeting = System.String;
using PersonalizedGreeting = System.String;

Func<Greeting, Name, PersonalizedGreeting> greet =
    (gr, name) => $"{gr}, {name}";

Name[] names = { "Tristan", "Ivan" };

names.Map(g => greet("Hello", g)).ForEach(WriteLine);
// prints: Hello, Tristan
//         Hello, Ivan
```

`greet` expects two arguments, and we're using *normal* function application.

Как можно отделить передачу "Hello" от вызова функции? We can solve this with *partial* application.
The idea is to allow some code to decide on the general greeting, giving that greeting to
`greet` as its first argument.
This will generate a new function with "Hello" already baked in as the greeting to use.

### 7.1.1 Manually enabling partial application

Rewrite the function like so:

```csharp
Func<Greeting, Func<Name, PersonalizedGreeting>> greetWith =
    gr => name => $"{gr}, {name}";
```

* `greetWith` takes a single argument and returns a new function of type `Name -> Greeting`.
* `gr` is captured in a closure and is therefore "remembered" until the returned function is called.

Usage:

```csharp
var greetFormally = greetWith("Good evening");
names.Map(greetFormally).ForEach(WriteLine);
// prints: Good evening, Tristan
//         Good evening, Ivan
```

Signatures:

```text
greet : (Greeting, Name) -> PersonalizedGreeting
greetWith : Greeting -> (Name -> PersonalizedGreeting)
```

Arrow notation is right-associative and the type of `greetWith` would normally be written as follows:

```text
greetWith : Greeting -> Name -> PersonalizedGreeting
```

`greetWith` is said to be in *curried* form; that is, all arguments are supplied one by one
via function invocation (вызов).

### 7.1.2 Generalizing partial application

Implementation of a general `Apply` function to a binary and ternary function: 

```csharp
static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> f, T1 t1) =>
    t2 => f(t1, t2);

static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> f, T1 t1) =>
    (t2, t3) => func(t1, t2, t3);
```

Usage:

```csharp
var greetInformally = greet.Apply("Hey");
names.Map(greetInformally).ForEach(WriteLine);
// prints: Hey, Tristan
//         Hey, Ivan
```

Partial application is always about going from general to specific:

1. We're starting with a *general* function (like `greet`).
2. Using partial application to create a *specialized* version of this function (like `greetInformally`).

### 7.1.3 Order of arguments matters

Порядок аргументов следует делать таким:

* Первыми следует определять аргументы, которые:
  * определяют как будет работать функция
  * зависимости, которые требуются функции для выполнения работы
* Затем определяются аргументы - сущности, на которые будет поздействовать операция.
Наиболее вероятно, что они будут получены и использованы в последнюю очередь.

## 7.2 Overcoming the quirks of method resolution (преодоление проблем при разрешении методов)

For compiler methods, lambdas, and delegates are all different things.

When we use `Option.Map`:

```csharp
Some(9.0).Map(Math.Sqrt)    // => 3.0
```

`Math.Sqrt` identifies a method, and `Map` expects a delegate of type `Func<T, R>`.
More precisely, `Math.Sqrt` identifies a "method group".

Compiler can infer the generic types of the `Func`s:

```csharp
Some(9.0).Map<double, double>(Math.Sqrt)
```

Unfortunately, for methods taking two or more arguments, all this goodness goes away
(перестает работать).

Example. Try to rewrite the `greet` function as a method `GreeterMethod`:

```csharp
// If we write our greeting function as a method...
PersonalizedGreeting GreeterMethod(Greeting gr, Name name) =>
    $"{gr}, {name}";

// ... then this expression does not compile.
Func<Name, PersonalizedGreeting> GreetWith(Greeting greeting) =>
    GreeterMethod.Apply(greeting);
```

This code doesn't compile because the name `GreeterMethod` identifies a `MethodGroup`,
whereas `Apply` expects a `Func`.

Если хочется использовать `Apply` для методов, то можно использовать одну из следующих форм:

```csharp
PersonalizedGreeting GreeterMethod(Greeting gr, Name name) =>
    $"{gr}, {name}";

// Provides all generic arguments explicitly. FuncExt.Apply - обычный Apply, который приведен выше.
Func<Name, PersonalizedGreeting> GreetWith_1(Greeting greeting) =>
    FuncExt.Apply<Greeting, Name, PersonalizedGreeting>(GreeterMethod, greeting);

// Explicitly converts the method to a delegate before calling Apply.
Func<Name, PersonalizedGreeting> GreetWith_2(Greeting greeting) =>
    new Func<Greeting, Name, PersonalizedGreeting>(GreeterMethod)
        .Apply(greeting);
```

Получается слишком громоздко. Лучше вместо методов использовать делегаты (`Func`):

```csharp
public class TypeInference_Delegate
{
    string separator = "! ";

    // 1. Field
    // Declaration and initialization of a delegate field;
    // note that you couldn't reference separator here.
    Func<Greeting, Name, PersonalizedGreeting> GreeterField =
        (gr, name) => $"{gr}, {name}";

    // 2. Property
    // A getter-only property has its body introduced by =>
    Func<Greeting, Name, PersonalizedGreeting> GreeterProperty =>
        (gr, name) => $"{gr}{separator}{name}";

    // 3. Factory
    // A method that acts as a factory of functions can have generic parameters.
    Func<Greeting, T, PersonalizedGreeting> GreeterFactory<T>() =>
        (gr, t) => $"{gr}{separator}{t}";
}
```

Их использование:

```csharp
GreeterField.Apply("Hi");
GreeterProperty.Apply("Hi");
GreeterFactory<Name>().Apply("Hi");
```

**Вывод**:

If you want to use HOF's that take multi-argument functions as arguments,
it's sometimes best to move away from using methods and write
`Func`s instead - or methods that return `Func`s.

## 7.3 Curried functions: optimized for partial application
