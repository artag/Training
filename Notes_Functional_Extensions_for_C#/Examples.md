# Examples

## AsyncUsageExamples

```csharp
public async Task<string>
    Promote_with_async_methods_in_the_beginning_of_the_chain(long id)
{
    var gateway = new EmailGateway();

    return await GetByIdAsync(id)
        .ToResult("Customer with such Id is not found: " + id)
        .Ensure(customer => customer.CanBePromoted(), "The customer has the highest status possible")
        .Tap(customer => customer.Promote())
        .Bind(customer => gateway.SendPromotionNotification(customer.Email))
        .Finally(result => result.IsSuccess ? "Ok" : result.Error);
}

public async Task<string>
    Promote_with_async_methods_in_the_beginning_and_in_the_middle_of_the_chain(long id)
{
    var gateway = new EmailGateway();

    return await GetByIdAsync(id)
        .ToResult("Customer with such Id is not found: " + id)
        .Ensure(customer => customer.CanBePromoted(), "The customer has the highest status possible")
        .Tap(customer => customer.PromoteAsync())
        .Bind(customer => gateway.SendPromotionNotificationAsync(customer.Email))
        .Finally(result => result.IsSuccess ? "Ok" : result.Error);
}

public async Task<string>
    Promote_with_async_methods_in_the_beginning_and_in_the_middle_of_the_chain_using_compensate(long id)
{
    var gateway = new EmailGateway();

    return await GetByIdAsync(id)
        .ToResult("Customer with such Id is not found: " + id)
        .Ensure(customer => customer.CanBePromoted(), "Need to ask manager")
        .OnFailure(error => Log(error))
        .OnFailureCompensate(() => AskManager(id))
        .Tap(customer => Log("Manager approved promotion"))
        .Tap(customer => customer.PromoteAsync())
        .Bind(customer => gateway.SendPromotionNotificationAsync(customer.Email))
        .Finally(result => result.IsSuccess ? "Ok" : result.Error);
}

void Log(string message)
{
}

Task<Result<Customer>> AskManager(long id)
{
    return Task.FromResult(Result.Success(new Customer()));
}

public Task<Maybe<Customer>> GetByIdAsync(long id)
{
    return Task.FromResult((Maybe<Customer>)new Customer());
}

public Maybe<Customer> GetById(long id)
{
    return new Customer();
}

public class Customer
{
    public string Email { get; }

    public Customer()
    {
    }

    public bool CanBePromoted()
    {
        return true;
    }

    public void Promote()
    {
    }

    public Task PromoteAsync()
    {
        return Task.FromResult(1);
    }
}

public class EmailGateway
{
    public Result SendPromotionNotification(string email)
    {
        return Result.Success();
    }

    public Task<Result> SendPromotionNotificationAsync(string email)
    {
        return Task.FromResult(Result.Success());
    }
}
```

## ExampleFromPluralsightCourse

```csharp
public string Promote(long id)
{
    var gateway = new EmailGateway();
    return GetById(id)
        .ToResult("Customer with such Id is not found: " + id)
        .Ensure(customer => customer.CanBePromoted(), "The customer has the highest status possible")
        .Tap(customer => customer.Promote())
        .Bind(customer => gateway.SendPromotionNotification(customer.Email))
        .Finally(result => result.IsSuccess ? "Ok" : result.Error);
}

public Maybe<Customer> GetById(long id)
{
    return new Customer();
}

public class Customer
{
    public string Email { get; }

    public Customer()
    {
    }

    public bool CanBePromoted()
    {
        return true;
    }

    public void Promote()
    {
    }
}

public class EmailGateway
{
    public Result SendPromotionNotification(string email)
    {
        return Result.Success();
    }
}
```

## ExampleWithOnFailureMethod

```csharp
public string OnFailure_non_async(int customerId, decimal moneyAmount)
{
    var paymentGateway = new PaymentGateway();
    var database = new Database();

    return GetById(customerId)
        .ToResult("Customer with such Id is not found: " + customerId)
        .Tap(customer => customer.AddBalance(moneyAmount))
        .Check(customer => paymentGateway.ChargePayment(customer, moneyAmount))
        .Bind(
            customer => database.Save(customer)
                .OnFailure(() => paymentGateway.RollbackLastTransaction()))
        .Finally(result => result.IsSuccess ? "OK" : result.Error);
}

private Maybe<Customer> GetById(long id)
{
    return new Customer();
}

private class Customer
{
    public void AddBalance(decimal moneyAmount)
    {
    }
}

private class PaymentGateway
{
    public Result ChargePayment(Customer customer, decimal moneyAmount)
    {
        return Result.Success();
    }

    public void RollbackLastTransaction()
    {
    }

    public Task RollbackLastTransactionAsync()
    {
        return Task.FromResult(1);
    }
}

private class Database
{
    public Result Save(Customer customer)
    {
        return Result.Success();
    }
}
```

## PassingResultThroughOnSuccessMethods

```csharp
public void Example1()
{
    Result<DateTime> result = FunctionInt()
        .Bind(x => FunctionString(x))
        .Bind(x => FunctionDateTime(x));
}

public void Example2()
{
    Result<DateTime> result = FunctionInt()
        .Bind(_ => FunctionString())
        .Bind(x => FunctionDateTime(x));
}

private Result<int> FunctionInt()
{
    return Result.Success(1);
}

private Result<string> FunctionString(int intValue)
{
    return Result.Success("Ok");
}

private Result<string> FunctionString()
{
    return Result.Success("Ok");
}

private Result<DateTime> FunctionDateTime(string stringValue)
{
    return Result.Success(DateTime.Now);
}
```

## TapExamples

```csharp
class Customer
{
    public string Name { get; set; }
}

class Error
{
}


// Action
void RaiseAlert() { }

// Func<Task>
Task RaiseAlertAsync() => Task.CompletedTask;

// Action<T>
void DeleteCustomer(Customer customer) { }

// Func<T, Task>
Task DeleteCustomerAsync(Customer customer) => Task.CompletedTask;

// Func<TResult>
int GetTotal() => 11;

// Func<Task<T>>
Task<int> GetTotalAsync() => Task.FromResult(11);

// Func<T, TResult>>
int AppendLog(string entry) => 332;

// Func<T, Task<TResult>>
Task<int> AppendLogAsync(string entry) => Task.FromResult(332);


// A method that returns a Result
Result DoWork() => Result.Success();
Task<Result> DoWorkAsync() => Task.FromResult(Result.Success());

// A method that returns a Result<T>
Result<Customer> GetCustomer(int customer) => Result.Success(new Customer());
Task<Result<Customer>> GetCustomerAsync(int customer) => Task.FromResult(Result.Success(new Customer()));

// A method that returns a Result<T, E>
Result<Customer, Error> GetCustomerB(int customer) => Result.Success<Customer, Error>(new Customer());
Task<Result<Customer, Error>> GetCustomerBAsync(int customer) => Task.FromResult(Result.Success<Customer, Error>(new Customer()));


public void ResultExtensions()
{
    var customer = new Customer();

    // Result
    DoWork()
        .Tap(RaiseAlert)
        .Tap(() => DeleteCustomer(customer))
        .Tap(() => GetTotal())                   // The int return value is ignored
        .Tap(() => AppendLog("log"))             // The int return value is ignored
        .Tap(() => customer.Name = "Matt")       // Inline lambda, external variable
        .Tap(() =>
        {                                        // Do whatever you want!
            DeleteCustomer(customer);
            GetTotal();
        });

    // Result<T>
    GetCustomer(21)                      // Result.Value contains the customer
        .Tap(RaiseAlert)
        .Tap(c => DeleteCustomer(c))     // Result.Value is passed as the parameter to the lambda
        .Tap(DeleteCustomer)             // Shorthand version of the above line, using method group conversion.
        .Tap(() => GetTotal())
        // .Tap(GetTotal)                // Method group conversion ONLY doesn't work for non-async parameterless Funcs.
        .Tap(() => AppendLog("log"))
        .Tap(c => c.Name = "Jenkins")
        .Tap(c =>
        {
            DeleteCustomer(c);
            GetTotal();
        });

    // Result<T, E>
    GetCustomerB(21)
        .Tap(RaiseAlert)
        .Tap(c => DeleteCustomer(c))
        .Tap(DeleteCustomer)
        .Tap(() => GetTotal())
        .Tap(() => AppendLog("log"))
        .Tap(c => c.Name = "Jenkins")
        .Tap(c =>
        {
            DeleteCustomer(c);
            GetTotal();
        });
}

public async Task AsyncResultExtensionsRightOperand()
{
    var customer = new Customer();

    // Result
    await DoWork()                              // await the entire chain
        .Tap(RaiseAlert)
        .Tap(() => RaiseAlertAsync())           // The first method to return a Task brings the entire chain into the Task<Result> space
        .Tap(() => DeleteCustomer(customer))    // Non-async methods can still be used interchangeably though
        .Tap(() => DeleteCustomerAsync(customer))
        .Tap(() => GetTotal())
        .Tap(() => GetTotalAsync())
        .Tap(GetTotalAsync)         // Since this is a Func (GetTotalAsync() returns a Task),
                                    // method group conversion works fine. The Task inner return value 
                                    // is discarded, of course, as with all the Tap methods.
        .Tap(() => AppendLog("log"))
        .Tap(() => AppendLogAsync("log"))
        .Tap(() => customer.Name = "Matt")
        .Tap(() =>
        {
            DeleteCustomer(customer);
            GetTotal();
        })
        .Tap(async () =>            // Even inline lambdas can be async!
        {
            await DeleteCustomerAsync(customer);
            await GetTotalAsync();
        });

    // Result<T>
    await GetCustomer(21)           // Result.Value contains the customer
        .Tap(RaiseAlert)
        .Tap(RaiseAlertAsync)
        .Tap(c => DeleteCustomer(c))
        .Tap(DeleteCustomer)
        .Tap(c => DeleteCustomerAsync(c))
        .Tap(DeleteCustomerAsync)
        .Tap(() => GetTotal())
        .Tap(GetTotalAsync)
        .Tap(() => AppendLog("log"))
        .Tap(() => AppendLogAsync("log"))
        .Tap(() => customer.Name = "Jenkins")
        .Tap(c =>
        {
            DeleteCustomer(c);
            GetTotal();
        })
        .Tap(async c =>
        {
            await DeleteCustomerAsync(c);
            await GetTotalAsync();
        });

    // Result<T, E>
    await GetCustomerB(21)
        .Tap(RaiseAlert)
        .Tap(RaiseAlertAsync)
        .Tap(DeleteCustomer)
        .Tap(DeleteCustomerAsync)
        .Tap(() => GetTotal())
        .Tap(GetTotalAsync)
        .Tap(() => AppendLog("log"))
        .Tap(() => AppendLogAsync("log"))
        .Tap(() => customer.Name = "Jenkins")
        .Tap(c =>
        {
            DeleteCustomer(c);
            GetTotal();
        })
        .Tap(async c =>
        {
            await DeleteCustomerAsync(c);
            await GetTotalAsync();
        });


    // Result
    await DoWork()
        .Tap(RaiseAlertAsync);

    // Result<T>
    await GetCustomer(21)
        .Tap(RaiseAlertAsync);

    await GetCustomer(21)
        .Tap(DeleteCustomerAsync);

    // Result<T, E>
    await GetCustomerB(21)
        .Tap(RaiseAlertAsync);

    await GetCustomerB(21)
        .Tap(DeleteCustomerAsync);
}

public async Task AsyncResultExtensionsLeftOperand()
{
    // Task<Result>
    await DoWorkAsync()
        .Tap(RaiseAlert);

    // Task<Result<T>>
    await GetCustomerAsync(21)
        .Tap(RaiseAlert);

    await GetCustomerAsync(21)
        .Tap(DeleteCustomer);

    // Task<Result<T, E>>
    await GetCustomerBAsync(21)
        .Tap(RaiseAlert);

    await GetCustomerBAsync(21)
        .Tap(DeleteCustomer);
}

public async Task AsyncResultExtensionsBothOperands()
{
    // Task<Result>
    await DoWorkAsync()
        .Tap(RaiseAlertAsync);

    // Task<Result<T>>
    await GetCustomerAsync(21)
        .Tap(RaiseAlertAsync);

    await GetCustomerAsync(21)
        .Tap(DeleteCustomerAsync);

    // Task<Result<T, E>>
    await GetCustomerBAsync(21)
        .Tap(RaiseAlertAsync);

    await GetCustomerBAsync(21)
        .Tap(DeleteCustomerAsync);
}
```
