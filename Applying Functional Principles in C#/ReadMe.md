# What Is Functional Programming

### Особенности:

* Same input – same result
* Information about possible inputs and outcomes

#### "Хороший код"

```csharp
public double Calculate(double x, double y)
{
    return x * x + y * y;
}
```

#### "Плохой код"

Result is always different

```csharp
public long TicksElapsedFrom(int year)
{
    DateTime now = DateTime.Now;
    DateTime then = new DateTime(year, 1, 1);

    return (now - then).Ticks;
}
```

#### "Плохой код" (Dishonest signature)

DivideByZeroException

```csharp
public static int Divide(int x, int y)
{
    return x / y;
}
```

#### "Хороший код"

Method Signature Honesty

```csharp
public static int Divide(int x, NonZeroInteger y)
{
    return x / y.Value;
}

// или

public static int? Divide(int x, int y)
{
    if (y == 0)
        return null;

    return x / y;
}
```

### Особенности:

* Honest - Has precisely defined input and output
* Referentially transparent - Doesn’t affect or refer to the global state


# Refactoring to an Immutable Architecture

**Immutability** - Inability to change data

**State** - Data that changes over time

**Side effect** - A change that is made to some state


#### "Плохой код"

Есть Side effects

```csharp
public class UserProfile
{
    private User _user;
    private string _address;

    // Здесь side effect
    public void UpdateUser(int userId, string name)
    {
        _user = new User(userId, name);
    }
}

public class User {
    public int Id { get; }
    public string Name { get; }

    public User(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
```


#### "Улучшенный код"

```csharp
public class UserProfile
{
    private readonly User _user;
    private readonly string _address;

    public UserProfile(User user, string address)
    {
        _user = user;
        _address = address;
    }

    public UserProfile UpdateUser(int userId, string name)
    {
        var newUser = new User(userId, name);
        return new UserProfile(newUser, _address);
    }
}

public class User {
    public int Id { get; }
    public string Name { get; }

    public User(int id, string name) {
        Id = id;
        Name = name;
    }
}
```

Пример см. также в solution `Immutability`, файл `UserProfile`.

Еще один пример см. в solution `Immutability`, файл `CustomerService`.


### Why Does Immutability Matter?

* Increased readability
* A single place for validating invariants
* Automatic thread safety


### Immutability Limitations

* CPU Usage - более высокая нагрузка на CPU
* Memory Usage - более высокое использование RAM

Как правило, не всегда удается полностью сделать все классы Immutable. 
Но к этому нужно стремиться.

```csharp
// Пример. (ImmutableList доступен через Nuget)
// Добавление нового элемента в immutable список, создает копию исходного списка.
ImmutableList<string> list = ImmutableList.Create<string>();
ImmutableList<string> list2 = list.Add("New item");

// Более грамотный пподход. Использование "гибридного" immutable списка.
ImmutableList<string>.Builder builder = ImmutableList.CreateBuilder<string>();
builder.Add("Line 1");
builder.Add("Line 2");
builder.Add("Line 3");
ImmutableList<string> immutableList = builder.ToImmutable();
```


### How to Deal with Side Effects (как бороться с побочными эффектами)

#### Решение 1

Command–query separation principle (CQS или CQRS). Разделение поведения на Command и Query.

**Особенности Command**
* Produces side effects (содержит побочные эффекты)
* Returns void (возвращает void)

**Особенности Query**
* Side-effect free (не содержит побочные эффекты)
* Returns non-void (что-то возвращает)

Пример:
```csharp
public class CustomerService
{
    // Command
    public void Process(string customerName, string addressString)
    {
        Address address = CreateAddress(addressString);
        Customer customer = CreateCustomer(customerName, address);
        SaveCustomer(customer);
    }

    // Query
    private Address CreateAddress(string addressString)
    {
        return new Address(addressString);
    }

    // Query
    private Customer CreateCustomer(string name, Address address)
    {
        return new Customer(name, address);
    }

    // Command
    private void SaveCustomer(Customer customer)
    {
        var repository = new Repository();
        repository.Save(customer);
    }
}
```

Еще пример. Метод действует как Command и Query одновременно:
```csharp
var stack = new Stack<string>();
stack.Push("value");             // Command
string value = stack.Pop();      // Both query and command
```

Не всегда удается полностью избавиться от двойного поведения методов (query and command).
По возможности следует стремиться к разделению поведения.

#### Решение 2

Приложение делится на две части:

* Domain Logic - Generates artifacts
* Mutating state ("внешний мир") - Uses artifacts to change the system's state

Правила:
* Make the mutable shell as dumb as possible
(Делать внешние mutable сервисы как можно более простыми)

* Apply side effect at the end of a business transaction
(Крайне желательно использовать mutable сервисы в конце операции)

```
Mutable Shell | --Input--> | Immutable Core | --Artifacts--> | Mutable Shell
```

### Пример Audit manager (проект Immutable)

Audit manager - immutable
Persister, Application service - mutable


# Refactoring Away from Exceptions


#### "Плохой код"

Типичный пример валидации

```csharp
public ActionResult CreateEmployee(string name)
{
    try
    {
        ValidateName(name);
        // Rest of the method

        return View("Success");
    }
    catch (ValidationException ex)
    {
        return View("Error", ex.Message);
    }
}

private void ValidateName(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ValidationException("Name cannot be empty");

    if (name.Length > 100)
        throw new ValidationException("Name is too long");
}
```

**Плохо:**

* Исключения, если их не поймать действуют как операция goto (даже еще хуже - исключение может
"перепрыгнуть" через несколько уровней стека).
* По сигнатуре метода не понятно, что может выкинуться исключение


#### "Хороший код"

1. Вместо выбрасывания исключений возвращаем строки с ошибками.
2. Сигнатура метода выглядит уже более пристойно (хотя все еще неидеально)
3. Срабатывание Validations это не Exceptional situation

```csharp
public ActionResult CreateEmployee(string name)
{
    string error = ValidateName(name);
    if (error != string.Empty)
        return View("Error", error);

    // Rest of the method
    return View("Success");
}

private string ValidateName(string name)
{
    if (name == null)
        throw new ArgumentNullException();

    if (string.IsNullOrWhiteSpace(name))
        return "Name cannot be empty";

    if (name.Length > 100)
        return "Name length cannot exceed 100 characters";

    return string.Empty;
}
```

Можно сделать еще лучше: использовать вместо `string` класс `Result` или `ResultWithEnum`
(см. далее).

См. код в проекте `Exceptions`, класс `EmployeeController`


### Правила

```
* Always prefer using return values over exceptions.
* Exceptions are for exceptional situations
* Exceptions should signalize a bug
* Don’t use exceptions in situations you expect to happen
```

Все необходимые проверки (наподобие приведенных чуть выше) выполнять на входных границах.
Например, для ASP.NET - это уровень `Controller`:

```csharp
// Уровень контроллера. Здесь производится валидация ввода.
public ActionResult UpdateEmployee(int employeeId, string name)
{
    var error = ValidateName(name);
    if (error != string.Empty)
        return View("Error", error);

    Employee employee = GetEmployee(employeeId);
    employee.UpdateName(name);
}

// Внутренний класс. Здесь валидация уже не требуется.
public class Employee
{
    public void UpdateName(string name)
    {
        // Эта ситуация уже нестандартная (аварийная) - выбрасывается исключение,
        // которое ловится на самом верхнем уровне.
        if (name == null)
            throw new ArgumentNullException();

        // ...
    }
}
```

Опять же, можно сделать еще лучше: использовать вместо `string` класс `Result` или
`ResultWithEnum` (см. далее).

См. код в проекте `Exceptions`, класс `EmployeeController`


### Fail Fast Principle

Особенности:

* Stopping the current operation
* More stable software


#### "Плохой код"

Fail Silently
1. Сокрытие проблемы
2. Ловится все

```csharp
public void ProcessItems(List<Item> items)
{
    foreach (Item item in items)
    {
        try
        {
            Process(item);
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }
}
```


#### "Хороший код"

Fail Fast

```csharp
public void ProcessItems(List<Item> items)
{
    foreach (Item item in items)
    {
        Process(item);
    }
}
```

### Правила:

```csharp
* Shortening the feedback loop
* Confidence in the working software
* Protects the persistence state
```

### Where to Catch Exceptions

1. **Необработанные исключения** (которые не предусмотренные) ловить на самом верхнем уровне:
Don’t put any domain logic here

```csharp
public static void Main()
{
    try
    {
        StartApplication();
    }
    catch (Exception ex)
    {
        LogException(ex);              // Запись в логи
        ShowGenericApology();          // Показать сообщение типа "что-то пошло не так..."
        Environment.FailFast(null);    // Прекращение работы
    }
}
```

Application’s type
* Stateful - Process shutdown 
* Stateless - Operation shutdown


2. **3-rd party library** выброс оттуда исключений

* Ловить исключения на как можно более низком уровне.
* Ловить только те исключения, которые мы знаем как обработать.

3. **Из нашего кода**

* Стараться не кидать исключения
* Плюс два правила из предыдущего пункта

Пример:

#### "Не очень хороший код"

Проблема: `DbUpdateException` не слишком "узкое" исключение - можем поймать слишком
много исключений различных видов.

```csharp
public void CreateCustomer(string name)
{
    Customer customer = new Customer(name);
    bool result = SaveCustomer(customer);

    if (!result)
    {
        MessageBox.Show("Error connecting to the database. Please try again later.");
    }
}

private bool SaveCustomer(Customer customer)
{
    try
    {
        using (MyContext context = new MyContext()) {
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        return true;
    }
    catch (DbUpdateException ex) {
        return false;
    }
}
```


#### "Более грамотный код"

1. Ловим исключения более избирательно
2. Возвращаем строки с информацией об ошибке
3. То, что не знаем как обработать, выкидываем на более верхние уровни.
4. Добавление возвращаемого `string` вместо обычного `bool` (см. примеры выше). 

```csharp
public void CreateCustomer(string name)
{
    var customer = new Customer(name);
    string result = SaveCustomer(customer);
    
    if (string.Empty)
    {
        MessageBox.Show("Error connecting to the database. Please try again later.");
    }
}

private string SaveCustomer(Customer customer)
{
    try
    {
        using (MyContext context = new MyContext())
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }
        return string.Empty;
    }
    catch (DbUpdateException ex)
    {
        if (ex.Message == "Unable to open the DB connection")
            return "Database is off-line";

        if (ex.Message.Contains("IX_Customer_Name"))
            return "Customer with such a name already exists";

        throw;
    }
}
```

Но можно сделать лучше - возвращать не `string`, а специальные классы:
**Result** или **ResultWithEnum** (см. решение `OperationResult`)

Их особенности:

* Helps keep methods honest
* Incorporates the result of an operation with its status
* Unified error model
* Only for expected failures

`Result` содержит строку с описанием ошибки.

Для более конкретной обработки ошибки можно использовать класс
`ResultWithEnum`, который содержит enum `ErrorType`


#### "Еще более грамотный код. Использование класса `Result`"

```csharp
public void CreateCustomer(string name)
{
    var customer = new Customer(name);
    Result result = SaveCustomer(customer);
    
    if (result.IsFailure)
    {
        MessageBox.Show(result.Error);
    }
}

private Result SaveCustomer(Customer customer)
{
    try
    {
        using (MyContext context = new MyContext())
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }
        return Result.Ok();
    }
    catch (DbUpdateException ex)
    {
        if (ex.Message == "Unable to open the DB connection")
            return Result.Fail("Database is off-line");

        if (ex.Message.Contains("IX_Customer_Name"))
            return Result.Fail("Customer with such a name already exists");

        throw;
    }
}

private Result<Customer> GetCustomer(int id)
{
    try
    {
        using (var context = new MyContext())
        {
            return Result.Ok(context.Customers.Single(x => x.Id == id));
        }
    }
    catch (DbUpdateException ex)
    {
        if (ex.Message == "Unable to open the DB connection")
            return Result.Fail<Customer>("Database is off-line");

        throw;
    }
}
```

См. пример в проекте `Exceptions`, класс `CustomerService`.


#### "И еще лучше и еще более грамотный код. Использование класса `ResultWithEnum`"

Более строгая фильтрация ошибок, используя enum `ErrorType`

```csharp
public void CreateCustomer(string name)
{
    var customer = new Customer(name);
    Result result = SaveCustomer(customer);

    // Более точный показ MessageBox по сравнению с предыдущим вариантом
    switch (result.ErrorType)
    {
        case ErrorType.DatabaseIsOffline:
            MessageBox.Show("Unable to connect to the database. Please try again later.");

        case ErrorType.CustomerAlreadyExists:
            MessageBox.Show("A customer with the name " + name + " already exists");
            break;

        default:
           throw new ArgumentException();
    }
}

private Result SaveCustomer(Customer customer)
{
    try
    {
        using (MyContext context = new MyContext())
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }
        return Result.Ok();
    }
    catch (DbUpdateException ex)
    {
        if (ex.Message == "Unable to open the DB connection")
            return Result.Fail(ErrorType.DatabaseIsOffline);

        if (ex.Message.Contains("IX_Customer_Name"))
            return Result.Fail(ErrorType.CustomerAlreadyExists);

        throw;
    }
}

private Result<Customer> GetCustomer(int id)
{
    try
    {
        using (var context = new MyContext())
        {
            return Result.Ok(context.Customers.Single(x => x.Id == id));
        }
    }
    catch (DbUpdateException ex)
    {
        if (ex.Message == "Unable to open the DB connection")
            return Result.Fail<Customer>(ErrorType.DatabaseIsOffline);

        throw;
    }
}
```

См. пример в проекте `Exceptions`, класс `CustomerService2`.


### The Result Class And CQS

```csharp
// Command.
// Not excepted to fail.
public void Save(Customer customer)
{
}

// Command.
// Excepted to fail.
public Result Save(Customer customer)
{
}

// Query.
// Not excepted to fail.
public Customer GetById(long id)
{
}

// Query.
// Excepted to fail.
public Result<Customer> GetById(long id)
{
}
```

Последний пример. Проект `Exceptions`, класс `TicketController`.


# Avoiding Primitive Obsession

Пример недостатков primitive obsession:
```csharp
public class User
{
    public string Email { get; }

    public User(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email should not be empty");

        email = email.Trim();
        if (email.Length > 256)
            throw new ArgumentException("Email is too long");

        if (!email.Contains("@"))
            throw new ArgumentException("Email is invalid");

        Email = email;
    }
}
```

и

```csharp
public class Organization
{
    public string PrimaryEmail { get; }
    public string SecondaryEmail { get; }

    public Organization(string primaryEmail, string secondaryEmail)
    {
        Validate(primaryEmail, secondaryEmail);

        PrimaryEmail = primaryEmail;
        SecondaryEmail = secondaryEmail;
    }

    private void Validate(params string[] emails)
    {
        /* Perform the validation here */
    }
}
```


### Drawbacks of Primitive Obsession

1. Makes code dishonest.

    Из сигнатуры метода не видно, что для задания почты требуется строка
особого вида, обладающая определенными свойствами).

2. Violates the DRY principle.

    Для каждого подобного почтового адреса (см. class `Organization`) требуются
похожие проверки, что приводит к дублированию кода.


### How to get rid of primitive obsession

Надо "обернуть" каждый из primitive obesession в отдельный класс.

Пример см. в решении `PrimitiveObsession`, класс `Email`.
Класс `Email` унаследован от `ValueObject<T>`.

Абстрактный класс `ValueObject<T>` (см. проект `PrimitiveObsession.Common`) введен для
удобства создания таких "оберток" для primitive obsession.


### Primitive Obsession and Defensive Programming

Есть такой код. Он явно содержит primitive obsession:

```csharp
public void ProcessUser(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(nameof(name));

    if (name.Trim().Length > 100)
        throw new ArgumentException(nameof(name));

    // Processing code
}

public void CreateUser(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(nameof(name));

    if (name.Trim().Length > 100)
        throw new ArgumentException(nameof(name));

    // Creation code
}

public void UpdateUser(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(nameof(name));

    if (name.Trim().Length > 100)
        throw new ArgumentException(nameof(name));

    // Update code
}
```

После добавления "обертки" вокруг primitive obsession все равно во всех
методах требуется проверка на `null`, но, зато, остальные проверки
инкапсулированы в одном месте - классе `UserName`
(см. в проекте `PrimitiveObsession`).

```csharp
public void ProcessUser(UserName name)
{
    if (name == null)
        throw new ArgumentException(nameof(name));

    // Processing code
}

public void CreateUser(UserName name)
{
    if (name == null)
        throw new ArgumentException(nameof(name));

    // Creation code
}

public void UpdateUser(UserName name)
{
    if (name == null)
        throw new ArgumentException(nameof(name));

    // Update code
}
```


### Primitive Obsession. Limitations

Не обязтально для каждого примитивного типа, который должен удовлетворять каким-либо правиламъ
создавать "обертку".

Например если в коде есть `moneyAmount` и оно может быть отрицательным, то этой переменной
может подойти и обычный тип `decimal`.


### Where to Convert Primitive Types into Value Objects?

Конвертация из примитивных типов в ValueObjects (как и валидация) должна происходить на границах
доменной модели (например, в ASP.NET контроллере):
```
| Outside world | -----------Validation----------> | Domain model |
|               | --Conversion to ValueObjects --> |              |
```

Внутри Domain model классы должны работать с ValueObjects:
```
| Domain class | <--ValueObjects--> | Domain class |
```

На выходе из Domain model должна происходить обратная конвертация, из ValueObject в
примитивный тип:
```
| Domain model | --Conversion to primitive --> | Outside world |
```

#### Примеры

**Двойная и ненужная конвертация (плохой код)**

```csharp
public void Process(string oldEmail, string newEmail)
{
    Result<Email> oldEmailResult = Email.Create(oldEmail);
    Result<Email> newEmailResult = Email.Create(newEmail);

    if (oldEmailResult.IsFailure || newEmailResult.IsFailure)
        return;

    string oldEmailValue = oldEmailResult.Value;
    Customer customer = GetCustomerByEmail(oldEmail);
    customer.Email = newEmail;
}
```

**Если внутри доменной модели работать только с ValueObjects (хороший код)**

```csharp
public void Process(Email oldEmail, Email newEmail)
{
    Customer customer = GetCustomerByEmail(oldEmail);
    customer.Email = newEmail;
}
```


**Большой пример** использования `ValueObject` смотреть в проекте `PrimitiveObsession`,
классы `CustomerController` и `Customer`.

Используются ValueObjects: классы `Email` и `CustomerName`.

Используемые общие базовые классы: `Result` и `ValueObject`.


# Avoiding Nulls with the Maybe Type

Проблемный код:
```csharp
// 1. Dishonest сигнатура метода GetById: он может возвратить как объект Organization,
//    так и null.
// 2. Отсюда, неясно точное значение organization 
Organization organization = _repository.GetById(id);


// Other code


// 3. Здесь, при вызове organization.Name возможен выброс NullReferenceException,
//    т.к. неизвестно точное значение organization.
// 4. Возможно будет сложная отладка, т.к. между объявлением и использованием organization
//    может выполняться и лежать куча разного кода.
// 5. Нарушение принципа Fail Fast
Console.WriteLine(organization.Name)
```


### Non-nullability on the Language Level

На данный момент, в компиляторе языке C# нет гарантированного способа определить является ли
ссылка на объект null или нет.

Возможно, какие-то проверки будут реализваны в будущих версиях языка.

### Mitigating the Billion-dollar Mistake
(Уменьшение вероятности наткнуться на NullReferenceException)

Введение класса `Maybe<T>` (или, в других источниках - `Optional<T>`)

```csharp
public Organization GetById(int id)
{
    // Some Code
}
```

Замена на:
```csharp
public Maybe<Organization> GetById(int id)
{
    // Some Code
}
```

Класс `Maybe<T>` действует похоже на класс `Nullable<T>` (который используется только для 
объектов типа struct (ValueObject)).

#### Полезные свойства Maybe_T и Nullable_T

1. Honest signature of the method. 
```csharp
public Maybe<Organization> GetById(int id)
{
    // Some Code
}

public static int? Divide(int x, int y)
{
    if (y == 0)
        return null;

    return x / y;
}
```

2. Safety in terms of convertions
```csharp
Maybe<Organization nullable = GetOrganozation(id);
Organization nonNullable = nullable;           // Error

Organization? nullable = GetOrganization(id);
Organization nonNullable = nullable;           // Error

Organization nonNullable = GetOrganization(id);
Maybe<Organization> nullable = nonNullable;    // Ok

Organization nonNullable = GetOrganization(id);
Organization? nullable = nonNullable;          // Ok
```


#### Где конвертировать объекты в/из Maybe_T

Как и в случае с `ValueObject` (см. предыдущий раздел) конвертацию данных необходимо осуществлять
до их входа в Domain Model и после их выхода во "внешний мир".

Внутри Domain Model все сущности должны использовать для работы объекты `Maybe<T>`.


#### Большой пример

Файл `Maybe<T>` можно найти в проекте `Nulls.Common`.

Используется `CustomerController` из предыдущего раздела с добавлением одного небольшого метода.

Правятся интерфейс `IDatabase` и метод `CustomerController.Index(int id)`.


### Добавление автоматической проверки на null. Использование `NullGuard.Fody`

*Внимание: в данном проекте я не пробовал это решение*.

Требуется установка NuGet пакета `NullGuard.Fody`.

После установки пакета в текущем проекте должен появиться файл `FodyWeavers.xml` со следующим
содержанием:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Weavers>
  <NullGuard IncludeDebugAssert="false" />
</Weavers>
```

`IncludeDebugAssert="false"` добавляется для проверки на ссылок на null в Debug режиме
(а не только в режиме Release).

Также, по умолчанию, проверка на null выполняется только для public методов. Чтобы включить
проверку для всех методов и свойств надо задать правило на уровне assembly.

Для этого создается файл в корне текущего проекта (в видео назван как `Initer.cs`) со следующим
содержанием:
```csharp
using NullGuard;

// Включить проверку для всех методов и аттрибутов.
[assembly: NullGuard(ValidationFlags.All)]

namespace Nulls
{
    public class Initer
    {
    }
}
```

Можно убрать проверки на null из конструкторов, методов и свойств.

Если надо передать в метод или свойство null, то ко входному параметру метода или свойства надо
добавить аттрибут `[AllowNull]`:

Примеры для `Maybe<T>`:
```csharp
private Maybe([AllowNull] T value)
{
    _value = value;
}

public static implicit operator Maybe<T>([AllowNull] T value)
{
    return new Maybe<T>(value);
}

// Выключение автоматической проверки на null
// как для входного, так и для выходного значений.

[return: AllowNull]
public T Unwrap([AllowNull] T defaultValue = default(T))
{
    if (HasValue)
        return Value;

    return defaultValue;
}
```


#### Ограничения на использование автоматической проверки на null

Нельзя использовать в проектах с библиотеками, которые широко используют (принимают/возвращают)
null. Например, это такие Application Services как:

* ASP.NET
* WPF
* WCF

В своем **отдельном** проекте, содержащим Domain Logic использовать `NullGuard.Fody`
можно и нужно.

Примерно так надо поступать со входными null ссылками из "внешнего мира":

```
|         | ASP.NET | (Conveting) |              |          | Внешний мир
| Null -> | WCF     | ---Maybe--> | Domain Model | --> Null | (DB)
|         | WPF     |             |              |          |
```


# Handling Failures and Input Errors in a Functional Way

Пример. См. проект `ErrorsAndFailures`, класс `ClassToRefactor`. Там есть такой код:
```csharp
public string RefillBalance(int customerId, decimal moneyAmount)
{
    Customer customer = _database.GetById(customerId);
    customer.Balance += moneyAmount;
    _paymentGateway.ChargePayment(customer.BillingInfo, moneyAmount);
    _database.Save(customer);

    return "Ok";
}
```

Этот код не годится для продакшена, надо добавить необходимые проверки.
Итак, "традиционный" путь:
```csharp
public string RefillBalance(int customerId, decimal moneyAmount)
{
    if (!IsMoneyAmountValid(moneyAmount))
    {
        _logger.Log("Money amount is invalid");
        return "Money amount is invalid";
    }

    Customer customer = _database.GetById(customerId);
    if (customer == null)
    {
        _logger.Log("Customer is not found");
        return "Customer is not found";
    }

    customer.Balance += moneyAmount;

    try
    {
        _paymentGateway.ChargePayment(customer.BillingInfo, moneyAmount);
    }
    catch (ChargeFailedException ex)
    {
        _logger.Log("Unable to charge the credit card");
        return "Unable to charge the credit card";
    }

    try
    {
        _database.Save(customer);
    }
    catch (Exception e)
    {
        _paymentGateway.RollbackLastTransaction();
        _logger.Log("Unable to connect to the database");
        return "Unable to connect to the database";
    }

    _logger.Log("OK");
    return "OK";
}
```

Очень сложно понять, что теперь происходит в этом коде.

Теперь применим приемы, описанные в предыдущих разделах:
* Обработка исключений, выбрасываемых сторонними библиотеками на как можно более низком уровне.

* Использование `Result` и его возврат вместо выброса исключений.

* Использование `Maybe`.

* Использование `ValueObject` вместо primitive obsession.

```csharp
public string RefillBalance(int customerId, decimal moneyAmount)
{
    // Refactoring. Применение ValueObject<T>.
    Result<MoneyToCharge> moneyToCharge = MoneyToCharge.Create(moneyAmount);
    if (moneyToCharge.IsFailure)
    {
        _logger.Log(moneyToCharge.Error);
        return moneyToCharge.Error;
    }

    // Refactoring. Применение Maybe<T>.
    Maybe<Customer> customer = _database.GetById(customerId);
    if (customer.HasNoValue)
    {
        _logger.Log("Customer is not found");
        return "Customer is not found";
    }

    // Refactoring. Преобразование изменения баланса.
    customer.Value.AddBalance(moneyToCharge.Value);

    // Refactoring.
    // 1. Перемещение try-catch блока на более низкий уровень (уровень ChargePayment).
    // 2. Возврат Result вместо выброса исключения.
    Result chargeResult = _paymentGateway.ChargePayment(customer.Value.BillingInfo, moneyToCharge.Value);

    if (chargeResult.IsFailure)
    {
        _logger.Log(chargeResult.Error);
        return chargeResult.Error;
    }

    // Refactoring.
    // 1. Перемещение try-catch блока на более низкий уровень (уровень Save).
    // 2. Возврат Result вместо выброса исключения.
    Result saveResult = _database.Save(customer.Value);
    if (saveResult.IsFailure)
    {
        _paymentGateway.RollbackLastTransaction();
        _logger.Log(saveResult.Error);
        return saveResult.Error;
    }

    _logger.Log("OK");
    return "OK";
}
```

Все равно, все еще очень сложно понять, что происходит в этом коде.

Надо использовать:


### Railway-oriented Programming

Основная идея - вызывать методы последовательно, по цепочке (*мое примечание:* получится как в LINQ).

Вводится 3 extension метода:
* `OnSuccess` - выполняется когда предыдущая операция успешно завершилась

```
Examine previous Result -> If Success    -> Execute method, return new Result
                        -> If No Success -> Return previous Result
```

* `OnFailure` - выполняется когда предыдущая операция завершилась с ошибкой

```
Examine previous Result -> If Success    -> Return previous Result
                        -> If No Success -> Execute method, return previous Result
```

* `OnBOth` - выполняется независимо от результата предыдущей операции

```
Get previous Result -> Execute method -> Return previous Result
```

Опять тот же пример. См. проект `ErrorsAndFailures`, класс `ClassToRefactor`.

Используется вспомогательный См. проект `ErrorsAndFailures`, класс `ResultExtensions`:

В сам класс `Result` добавился метод `Combine` для проверки нескольких операций на их успешное
завершение (см. проект `OperationalResult`, класс `Result`).

В результате проведенных преобразований, код (метод) в классе `ClassToRefactor` выглядит так:

```csharp
public string RefillBalance(int customerId, decimal moneyAmount)
{
    Result<MoneyToCharge> moneyToCharge = MoneyToCharge.Create(moneyAmount);
    Result<Customer> customer = _database.GetById(customerId).ToResult("Customer is not found");

    return Result.Combine(moneyToCharge, customer)
        .OnSuccess(() => customer.Value.AddBalance(moneyToCharge.Value))
        .OnSuccess(() => _paymentGateway.ChargePayment(customer.Value.BillingInfo, moneyToCharge.Value))
        .OnSuccess(
            () => _database.Save(customer.Value)
                .OnFailure(() => _paymentGateway.RollbackLastTransaction()))
        .OnBoth(result => Log(result))
        .OnBoth(result => result.IsSuccess ? "OK" : result.Error);
}

private void Log(Result result)
{
    if (result.IsFailure)
        _logger.Log(result.Error);
    else
        _logger.Log("OK");
}
```


#### Замечание

* Railway-oriented programming подходит только для относительно простых действий.
Если есть что-то сложное, то придется возвращаться к классическим `if...else`
конструкциям.

* Все расширения из `ResultExtensions` можно релизовать в самом `Result`, но это действие может
привести к засорению последнего.
