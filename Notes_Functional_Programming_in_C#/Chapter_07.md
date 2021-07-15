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

Разбивание больших классов на более мелкие компоненты с определенными зонами ответственности
приводит к:
* код становится более модульным и управляемым
* код становится легче повторно использовать

Но, перед "употреблением", разделенные компоненты необходимо собирать/компоновать.

### 7.5.1 Modularity in OOP

Modularity in OOP is usually obtained by assigning responsibilities to different objects,
and capturing these responsibilities with interfaces.

Example. Interfaces in OOP capture the components responsibilities:

```csharp
public interface IValidator<T>
{
    Validation<T> Validate(T request);
}

public interface IRepository<T>
{
    Option<T> Lookup(Guid id);
    Exceptional<Unit> Save(T entity);
}
```

A higher-level component (controller) consumes lower-level components via interfaces.
This pattern called *dependency inversion*.

Преимущества такого подхода:

* *Decoupling* (разделение/развяка) - You could swap out the repository implementation
(changing it from writing to a database to writing to a queue) and this wouldn't impact the
controller. You'd only need to change how the two are wired up. (This is usually
defined in some bootstrapping logic.)
* *Testability* - You can unit-test the handler without hitting the database, by injecting a fake
*IRepository*.

И недостатки:

* There's an explosion in the number of interfaces, adding boilerplate and making the code
difficult to navigate.

* The bootstrapping logic to compose the application is often not trivial.

* Building fake implementations for testability can be complex.

Для управления всем этим могут применяться IoC containers and mocking frameworks.

Итак, пример с прошлой главы. An implementation that's functional in the small and OO in the large:

```csharp
public class BookTransferController : Controller
{
    IValidator<BookTransfer> validator;
    IRepository<BookTransfer> repository;

    public BookTransferController(
        IValidator<BookTransfer> validator, IRepository<BookTransfer> repository)
    {
        this.validator = validator;
        this.repository = repository;
    }

    [HttpPost, Route("api/transfers/book")]
    public IActionResult TransferOn([FromBody] BookTransfer cmd) =>
        validator.Validate(cmd)
            .Map(repository.Save)
            .Match(
                Invalid: BadRequest,
                Valid: result => result.Match<IActionResult>(
                    Exception: _ => StatusCode(500, Errors.UnexpectedError),
                    Success: _ => Ok()));
}
```

The main components (controller, validator, repository) are indeed objects.

Many functional concepts are then used in the implementation of the methods and in defining
their signatures.

### 7.5.2 Modularity in FP

In OOP fundamental units are objects, in FP they're functions. В ФП в качестве зависимостей
используются другие функции.

Example. Injecting functions as dependencies (было в главе 2):

```csharp
public class DateNotPast : IValidator<BookTransfer>
{
    Func<DateTime> clock;

    public DateNotPastValidator(Func<DateTime> clock) { this.clock = clock; }

    public Validation<BookTransfer> Validate(BookTransfer cmd) =>
        cmd.Date.Date < clock().Date
            ? Errors.TransferDateIsPast
            : Valid(cmd);
}
```

Зачем нам интерфейс `IValidator`? Let's instead use a delegate to represent validation:

```csharp
// T -> Validation<T>
public delegate Validation<T> Validator<T>(T t);
```

Теперь класс будет зависеть не от объекта `IValidator`, а от функции `Validator`.

Dependencies can be passed as arguments to a function:

```csharp
public static Validator<BookTransfer> DateNotPast(Func<DateTime> clock) =>
    cmd => cmd.Date.Date < clock().Date
        ? Errors.TransferDateIsPast
        : Valid(cmd);
```

`DateNotPast` is a HOF that takes a function `clock` and returns a function of type Validator.

Let's see how you would create a `Validator`. When bootstrapping the application,
you'd give `DateNotPast` a function that reads from the system clock:

```csharp
Validator<BookTransfer> val = DateNotPast(() => DateTime.UtcNow())
```

For testing purposes, however, you can provide a clock that returns a constant date:

```csharp
var uut = DateNotPast(() => new DateTime(2020, 20, 10));
```

This is in fact partial application:

1. `DateNotPast` is a binary function (in curried form) that needs a clock and a command
to compute its result. 

2. You supply the first argument when composing the application (or in the *arrange* phase of a unit
test), and the second argument when actually processing the received request (or in
the *act* phase of a unit test).

`BookTransferController` also needs a dependency to persist the `BookTransfer` request data.
If we use functions, we can represent this with the following signature:

```text
BookTransfer -> Exceptional<Unit>
```

Again, on start we create a very general function that writes to the DB, with this signature:

```text
TryExecute : ConnectionString -> SqlTemplate -> object -> Exceptional<Unit>
```

Then parameterize it with a connection string from configuration and a SQL template with
the command we want to execute.

Our controller implementation will now look like this:

```csharp
public class BookTransferController : Controller
{
    Validator<BookTransfer> validate;
    Func<BookTransfer, Exceptional<Unit>> save;

    [HttpPut, Route("api/transfers/book")]
    public IActionResult BookTransfer([FromBody] BookTransfer cmd) =>
        validate(cmd)
            .Map(save)
            .Match( //...
}
```

Why we need a controller class at all, when all the logic we're using could be captured in a
function of this type:

```text
BookTransfer -> IActionResult
```

Indeed (действительно), we could define such a function outside the scope of a controller
and configure the ASP.NET request pipeline to run it (см. линки).

Но пока это делать не рекомендуется, т.к. ASP.NET не очень хорошо поддерживает такой стиль обработки
HTTP запросов. Поэтому пока рекомендуется использовать стандартные `Controller`'s.

### 7.5.3 Comparing the two approaches

В примере выше все зависимости контроллера функции.

Преимущества такого подхода (первые два из dependency inversion, из ООП):

1. *Decoupling* - The controller knows nothing about the implementation details of
the functions it consumes.

2. *Testability* - When testing a controller method, you can just pass it functions that
return a predictable result.

3. You don't need to define any interfaces.

4. This makes testing easier, because you don't need to set up fakes.

5. Легче следовать подходу *interface segregation principle* (ISP).

Example 1. Unit test. When dependencies are functions, unit tests can be written without fakes:

```csharp
[Test]
public void WhenCmdIsValid_AndSaveSucceeds_ThenResponseIsOk()
{
    // (1),(2) - Injects functions that return a predictable result
    var controller = new BookTransferController(
        validate: cmd => Valid(cmd),                //(1)
        save: _ => Exceptional(Unit()));            //(2)

    var result = controller.BookTransfer(new BookTransfer());
    Assert.AreEqual(typeof(OkResult), result.GetType());
}
```

Example 2. Подход *interface segregation principle* (ISP).

Контроллер использует только метод `Save` из интерфейса `IRepository<T>`:

```csharp
public interface IRepository<T>
{
    Option<T> Lookup(Guid id);
    Exceptional<Unit> Save(T entity);
}
```

Для контроллера лучше всего выделить более специализированный интерфейс:

```csharp
public interface ISaveToRepository<T>
{
    Exceptional<Unit> Save(T entity);
}
```

Но это приведет к разрастанию числа интерфейсов в приложении. Проще всего использовать
функциональный подход к инверсии зависимостей.

### 7.5.4 Composing the application

Composing the services required to fulfill (выполнения) the `BookTransfer` request:

```csharp
// IControllerActivator in ASP.NET define bootstrapping logic.
public class ControllerActivator : IControllerActivator
{
    IConfigurationRoot configuration;
    public object Create(ControllerContext context)
    {
        var type = context.ActionDescriptor.ControllerTypeInfo;
        if (type.AsType().Equals(typeof(BookTransferController)))
            return ConfigureBookTransferController();
        //...
    }

    BookTransferController ConfigureBookTransferController()
    {
        ConnectionString connString = configuration.GetSection("ConnectionString").Value;

        // Sets up persistence
        var save = Sql.TryExecute
            .Apply(connString)
            .Apply(Sql.Queries.InsertTransferOn);

        // Sets up validation
        var validate = Validation.DateNotPast(() => DateTime.UtcNow);

        return new BookTransferController(validate, save);
    }
}
```

Осталось решить только одну проблему: как сделать *composite validator*
(несколько правил валидаций) в подходе ФП?

## 7.6 Reducing a list to a single value

In FP operation *fold* or *reduce* - reducing a list of values into a single value.

LINQ uses a different name: *Aggregate*.

### 7.6.1 LINQ’s Aggregate method

`Aggregate` takes a list of `n` things and returns exactly one thing (just like the SQL aggregate
functions `COUNT`, `SUM`, and `AVERAGE`).

`Aggregate` uses:

* *accumulator* - initial value
* *reducer function* - a binary function accepting the accumulator and an element in
the list, and returning the new value for the accumulator.

The signature for Aggregate is:

```text
(IEnumerable<T>, Acc, ((Acc, T) -> Acc)) -> Acc
```

The `Sum` function (in LINQ) is a special case of `Aggregate`:
* `0` - initial accumulator value.
* The binary function is addition (сложение).
 
Можно выразить `Sum` через `Aggregate` таким образом:

```csharp
Range(1, 5).Aggregate(0, (acc, i) => acc + i)    // => 15
```

Более наглядно:

```text
((((0 + 1) + 2) + 3) + 4) + 5   или    f(f(f(f(acc, t0), t1), t2), ... tn)
```

`Count` can also be seen as a special case of `Aggregate`:

```csharp
Range(1, 5).Aggregate(0, (count, _) => count + 1)    // => 5
```

Тип accumulator не обязательно должен быть типом list item. Пример использования
`Aggregate` с accumulator в виде объекта `Tree<T>` (item'ы добавляются в `Tree<T>`):

```csharp
Range(1, 5).Aggregate(Tree<int>.Empty, (tree, i) => tree.Insert(i))
```

`Aggregate` can implement `Map`, `Where` and `Bind` methods.

Также есть вариант использования `Aggregate` без аргумента accumulator. Вместо accumulator
используется первый элемент list'а.

Его signature:

```text
(IEnumerable<T>, ((T, T) -> T)) -> T)
```

### 7.6.2 Aggregating validation results

При помощи `Aggregate` можно "reduce" список validators до одного validator:

```text
IEnumerable<Validator<T>> -> Validator<T>               // (1)

// T -> Validation<T>
public delegate Validation<T> Validator<T>(T t);        // (2)

// Из (1) и (2) следует сигнатура функции, которую надо реализовать для "reduce":
IEnumerable<T -> Validation<T>> -> T -> Validation<T>
```

Есть два способа "reduce":

* *Fail fast* - the combined validation should fail as soon as one validator fails.

* *Harvest errors* - identify all the rules that have been violated.

### Fail fast

Using Aggregate and Bind to apply all validation in a sequence:

```csharp
public static Validator<T> FailFast<T>(IEnumerable<Validator<T>> validators) =>
    t => validators.Aggregate(Valid(t), (acc, validator) => acc.Bind(_ => validator(t)));
```

Здесь:

* `Valid(t)` - accumulator
* Applies each validator in the list to the accumulator with `Bind`.

Мысленно `Aggregate` можно представить так:

```csharp
Valid(t)
    .Bind(validators[0]))
    .Bind(validators[1]))
    ...
    .Bind(validators[n - 1]));
```

**Рекомендация** - сначала использовать наиболее быстрые проверки (валидаторы) и только потом
более медленные.

### 7.6.3 Harvesting validation errors

