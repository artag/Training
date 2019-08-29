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

