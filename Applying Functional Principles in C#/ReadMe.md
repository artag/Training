# What Is Functional Programming

### �����������:

* Same input � same result
* Information about possible inputs and outcomes

<span style="color:green">"������� ���"</span>

```csharp
public double Calculate(double x, double y)
{
    return x * x + y * y;
}
```

<span style="color:red">"������ ���"</span>

Result is always different

```csharp
public long TicksElapsedFrom(int year)
{
    DateTime now = DateTime.Now;
    DateTime then = new DateTime(year, 1, 1);

    return (now - then).Ticks;
}
```

<span style="color:red">"������ ���" (Dishonest signature)</span>

DivideByZeroException

```csharp
public static int Divide(int x, int y)
{
    return x / y;
}
```

<span style="color:green">"������� ���"</span>

Method Signature Honesty

```csharp
public static int Divide(int x, NonZeroInteger y)
{
    return x / y.Value;
}

// ���

public static int? Divide(int x, int y)
{
    if (y == 0)
        return null;

    return x / y;
}
```

### �����������:

* Honest - Has precisely defined input and output
* Referentially transparent - Doesn�t affect or refer to the global state


# Refactoring to an Immutable Architecture

**Immutability** - Inability to change data
**State** - Data that changes over time
**Side effect** - A change that is made to some state

<span style="color:red">"������ ���"</span>

���� Side effects

```csharp
public class UserProfile {
    private User _user;
    private string _address;

    public void UpdateUser(int userId, string name) {
        _user = new User(userId, name);
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

<span style="color:green">"���������� ���"</span>

```csharp
public class UserProfile {
    private readonly User _user;
    private readonly string _address;

    public UserProfile(User user, string address) {
        _user = user;
        _address = address;
    }

    public UserProfile UpdateUser(int userId, string name) {
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


### Why Does Immutability Matter?

* Increased readability
* A single place for validating invariants
* Automatic thread safety


### Immutability Limitations

* CPU Usage 
* Memory Usage

```csharp
// ������. ���������� ������ �������� � immutable ������, ������� ����� ��������� ������
ImmutableList<string> list = ImmutableList.Create<string>();
ImmutableList<string> list2 = list.Add("New item");

// ����� ��������� ������. ������������� "����������" immutable ������.
ImmutableList<string>.Builder builder = ImmutableList.CreateBuilder<string>();
builder.Add("Line 1");
builder.Add("Line 2");
builder.Add("Line 3");
ImmutableList<string> immutableList = builder.ToImmutable();
```


# How to Deal with Side Effects

Command�query separation principle

**Command**
* Produces side effects
* Returns void

**Query**
* Side-effect free
* Returns non-void

������:
```csharp
public class CustomerService {
    // Command
    public void Process(string customerName, string addressString) {
        Address address = CreateAddress(addressString);
        Customer customer = CreateCustomer(customerName, address);
        SaveCustomer(customer);
    }

    // Query
    private Address CreateAddress(string addressString) {
        return new Address(addressString);
    }

    // Query
    private Customer CreateCustomer(string name, Address address) {
        return new Customer(name, address);
    }

    // Command
    private void SaveCustomer(Customer customer) {
        var repository = new Repository();
        repository.Save(customer);
    }
}
```

��� ������:
```csharp
var stack = new Stack<string>();
stack.Push("value");             // Command
string value = stack.Pop();      // Both query and command
```

���������� ������� �� ��� �����:
* Domain Logic - Generates artifacts
* Mutating state ("������� ���") - Uses artifacts to change the system's state

�������:
* Make the mutable shell as dumb as possible
(������ ������� mutable ������� ��� ����� ����� ��������)

* Apply side effect at the end of a business transaction
(������ ���������� ������������ mutable ������� � ����� ��������)

### ������ Audit manager (������ Immutable)

Audit manager - immutable
Persister, Application service - mutable


# Refactoring Away from Exceptions

<span style="color:red">"������ ���"</span>

�������� ������ ���������

```csharp
public ActionResult CreateEmployee(string name) {
    try {
        ValidateName(name);
        // Rest of the method

        return View("Success");
    }
    catch (ValidationException ex) {
        return View("Error", ex.Message);
    }
}

private void ValidateName(string name) {
    if (string.IsNullOrWhiteSpace(name))
        throw new ValidationException("Name cannot be empty");

    if (name.Length > 100)
        throw new ValidationException("Name is too long");
}
```

**�����:**

* ����������, ���� �� �� ������� ��������� ��� �������� goto (���� ��� ���� - ���������� �����
"������������" ����� ��������� ������� �����).
* �� ��������� ������ �� �������, ��� ����� ���������� ����������


<span style="color:green">"������� ���"</span>

1. ������ ������������ ���������� ���������� ������ � ��������.
2. ��������� ������ �������� ��� ����� ��������� (���� ��� ��� ����������)
3. ������������ Validations ��� �� Exceptional situation

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

����� ������� ��� �����: ������������ ������ `string` ����� `Result` ��� `ResultWithEnum`
(��. �����).

��. ��� � ������� `Exceptions`, ����� `EmployeeController`

### �������

```
* Always prefer using return values over exceptions.
* Exceptions are for exceptional situations
* Exceptions should signalize a bug
* Don�t use exceptions in situations you expect to happen
```

��� ����������� �������� (��������� ����������� ���� ����) ��������� �� ������� ��������.
��������, ��� ASP.NET - ��� ������� `Controller`:

```csharp
// ������� �����������. ����� ������������ ��������� �����.
public ActionResult UpdateEmployee(int employeeId, string name)
{
    var error = ValidateName(name);
    if (error != string.Empty)
        return View("Error", error);

    Employee employee = GetEmployee(employeeId);
    employee.UpdateName(name);
}

// ���������� �����. ����� ��������� ��� �� ���������.
public class Employee
{
    public void UpdateName(string name)
    {
        // ��� �������� ��� ������������� (���������) - ������������� ����������,
        // ������� ������� �� ����� ������� ������.
        if (name == null)
            throw new ArgumentNullException();

        // ...
    }
}
```

����� ��, ����� ������� ��� �����: ������������ ������ `string` ����� `Result` ���
`ResultWithEnum` (��. �����).

��. ��� � ������� `Exceptions`, ����� `EmployeeController`

### Fail Fast Principle

�����������:

* Stopping the current operation
* More stable software

<span style="color:red">"������ ���"</span>

Fail Silently
1. �������� ��������
2. ������� ���

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

<span style="color:green">"������� ���"</span>

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

### �������:

```csharp
* Shortening the feedback loop
* Confidence in the working software
* Protects the persistence state
```

### Where to Catch Exceptions

1. **�������������� ����������** (������� �� ���������������) ������ �� ����� ������� ������:
Don�t put any domain logic here

```csharp
public static void Main()
{
    try
    {
        StartApplication();
    }
    catch (Exception ex)
    {
        LogException(ex);              // ������ � ����
        ShowGenericApology();          // �������� ��������� ���� "���-�� ����� �� ���..."
        Environment.FailFast(null);    // ����������� ������
    }
}
```

Application�s type
* Stateful - Process shutdown 
* Stateless - Operation shutdown


2. **3-rd party library** ������ ������ ����������

* ������ ���������� �� ��� ����� ����� ������ ������.
* ������ ������ �� ����������, ������� �� ����� ��� ����������.

3. **�� ������ ����**

* ��������� �� ������ ����������
* ���� ��� ������� �� ����������� ������

������:

<span style="color:red">"�� ����� ������� ���"</span>

��������: `DbUpdateException` �� ������� "�����" ���������� - ����� ������� �������
����� ���������� ��������� �����.

```csharp
public void CreateCustomer(string name) {
    Customer customer = new Customer(name);
    bool result = SaveCustomer(customer);

    if (!result) {
        MessageBox.Show("Error connecting to the database. Please try again later.");
    }
}

private bool SaveCustomer(Customer customer) {
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

<span style="color:green">"����� ��������� ���"</span>

1. ����� ���������� ����� ������������
2. ���������� ������ � ����������� �� ������
3. ��, ��� �� ����� ��� ����������, ���������� �� ����� ������� ������.
4. ���������� ������������� `string` ������ �������� `bool` (��. ������� ����). 

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

�� ����� ������� ����� - ���������� �� `string`, � ����������� ������:
**Result** ��� **ResultWithEnum** (��. ������� `OperationResult`)

�� �����������:

* Helps keep methods honest
* Incorporates the result of an operation with its status
* Unified error model
* Only for expected failures

`Result` �������� ������ � ��������� ������.

��� ����� ���������� ��������� ������ ����� ������������ �����
`ResultWithEnum`, ������� �������� enum `ErrorType`

<span style="color:green">"��� ����� ��������� ���. ������������� ������ `Result`"</span>

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

��. ������ � ������� `Exceptions`, ����� `CustomerService`.

<span style="color:green">"� ��� ����� � ��� ����� ��������� ���. ������������� ������ `ResultWithEnum`"</span>

����� ������� ���������� ������, ��������� enum `ErrorType`

```csharp
public void CreateCustomer(string name)
{
    var customer = new Customer(name);
    Result result = SaveCustomer(customer);

    // ����� ������ ����� MessageBox �� ��������� � ���������� ���������
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

��. ������ � ������� `Exceptions`, ����� `CustomerService2`.

