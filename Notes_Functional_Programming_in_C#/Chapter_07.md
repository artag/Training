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

*Currying* is the process of transforming an `n`-ary function `f` that takes arguments
`t1`, `t2`, ..., tn into a unary function that takes `t1` and yields a new function that takes `t2`,
and so on. 

In other words, `n`-ary function with signature

```text
(T1, T2, ..., Tn) -> R
```

when curried, has signature:

```text
T1 -> T2 -> ... -> Tn -> R
```

This functions

```csharp
Func<Greeting, Name, PersonalizedGreeting> greet
= (gr, name) => $"{gr}, {name}";

Func<Greeting, Func<Name, PersonalizedGreeting>> greetWith
= gr => name => $"{gr}, {name}";
```

has signatures:

```text
greet : (Greeting, Name) -> PersonalizedGreeting
greetWith : Greeting -> Name -> PersonalizedGreeting
```

And we can use this curried function like this:

```csharp
// Could call the curried function like so:
greetWith("hello")("world")     // => "hello, world"

// Partial application
var greetFormally = greetWith("Good evening");      // greetWith called manual currying
names.Map(greetFormally).ForEach(WriteLine);
// prints: Good evening, Tristan
//         Good evening, Ivan
```

For binary and ternary functions, generic `Curry` looks like this:

```csharp
static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func) =>
        t1 => t2 => func(t1, t2);

static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func) =>
    t1 => t2 => t3 => func(t1, t2, t3);
```

Use generic `Curry`:

```csharp
var greetWith = greet.Curry();
var greetNostalgically = greetWith("Arrivederci");
names.Map(greetNostalgically).ForEach(WriteLine);
// prints: Arrivederci, Tristan
//         Arrivederci, Ivan
```

Differences between partial application and currying:

* *Partial application* - You give a function fewer (меньше) arguments than the function
expects, obtaining a function that's particularized (уточняется/задается) with the values
of the arguments given so far.

* *Currying* - You don't give any arguments; you just transform an `n`-ary function
into a unary function, to which arguments can be successively given to eventually get
the same result as the original function.

**Summary** - ways of using partial application:

1. By writing functions in curried form.
2. By currying functions with `Curry`, and then invoking the curried function with
subsequent arguments.
3. By supplying arguments one by one with `Apply`.

## 7.4 Creating a partial-application-friendly API

Example - accessing a SQL database (using Dapper (см. главу 1)).

We'd need to implement functions of these types:

```text
lookupEmployee : Guid -> Option<Employee>
findEmployeesByLastName : string -> IEnumerable<Employee>
```

For retrieving data, Dapper exposes the Query method with the signature:

```csharp
public static IEnumerable<T> Query<T>(
    this IDbConnection conn, string sqlQuery, object param = null,
    SqlTransaction tran = null, bool buffered = true)
```

* `T` - returned data by the query. In our case `Employee`.

* `conn` - the connection to the database. 

* `sqlQuery` - SQL query template. (example: `"SELECT * FROM EMPLOYEES WHERE ID = @Id"`)

* `param` - object whose properties will be used to populate the placeholders in the `sqlQuery`.

### 7.4.1 Types as documentation

A custom type for connection strings:

```csharp
public class ConnectionString
{
    string Value { get; }
    public ConnectionString(string value) { Value = value; }

    // Implicit conversion to and from string
    public static implicit operator string(ConnectionString c) => c.Value;
    public static implicit operator ConnectionString(string s) => new ConnectionString(s);

    public override string ToString() => Value;
}
```

Точно такой же тип можно задать и для SQL template - класс `SqlTemplate`.

При старте приложения `ConnectionString` можно сконфигурировать подобным образом:

```csharp
ConnectionString connString = configuration.GetSection("ConnectionString").Value;
```

Function signatures are more explicit when using custom types:

```csharp
public Option<Employee> lookupEmployee(ConnectionString conn, Guid id) => //...
```

Плюсы:

1. `ConnectionString` гораздо нагляднее чем просто строка `string`.

2. We can now define extension methods on `ConnectionString`, что не имело особого смысла для строки.

### 7.4.2 Particularizing the data access function

An adapter function that's better suited for partial application:

```csharp
using static ConnectionHelper;
public static class ConnectionStringExt
{
    public static Func<SqlTemplate, object, IEnumerable<T>> Query<T>(
        this ConnectionString connString) =>
            (sql, param) => Connect(connString, conn => conn.Query<T>(sql, param));
}

// Where ConnectionHelper.Connect, which we implemented in chapter 1:
public static class ConnectionHelper
{
    public static R Connect<R>(string connString, Func<IDbConnection, R> f)
    {
        using (var conn = new SqlConnection(connString))    // Setup
        {                                                   // Setup
            conn.Open();                                    // Setup
            return f(conn);
        }                                                   // Teardown
    }
}
```

Signature of the preceding method: `ConnectionString -> (SqlTemplate, object) -> IEnumerable<T>`

This definition of Query is a thin shim (тонкая прокладка) on top of Dapper's Query function.
It provides a partial-application friendly API, for two reasons:

* Arguments this time truly go from general to specific.

* Supplying the first argument yields a `Func`, which resolves the issues of type
inference when applying subsequent arguments.

Supplying arguments to get a function of the desired signature:

```csharp
// Comments
// (1) - The connection string and retrieved type are fixed.
// (2) - The SQL query to be used is fixed.
// (3) - The functions we set out to implement.

ConnectionString connString = configuration
    .GetSection("ConnectionString").Value;

SqlTemplate sel = "SELECT * FROM EMPLOYEES";
SqlTemplate sqlById = $"{sel} WHERE ID = @Id";
SqlTemplate sqlByName = $"{sel} WHERE LASTNAME = @LastName";

// (SqlTemplate, object) -> IEnumerable<Employee>
var queryEmployees = conn.Query<Employee>();                        // (1)

// object -> IEnumerable<Employee>
var queryById = queryEmployees.Apply(sqlById);                      // (2)

// object -> IEnumerable<Employee>
var queryByLastName = queryEmployees.Apply(sqlByName);              // (2)

// Guid -> Option<Employee>
Option<Employee> lookupEmployee(Guid id) =>                         // (3)
    queryById(new { Id = id }).FirstOrDefault();

// string -> IEnumerable<Employee>
IEnumerable<Employee> findEmployeesByLastName(string lastName) =>   // (3)
    queryByLastName(new { LastName = lastName });
```

## 7.5 Modularizing and composing an application
