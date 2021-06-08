# Result

## Extensions

### Bind

* Selects result from the return value of a given function.
* If the calling Result is a failure, a new failure result is returned instead.

```csharp
Result<TO, TE> Bind<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, Result<TO, TE>> func)
{
    if (result.IsFailure)
        return Result.Failure<TO, TE>(result.Error);

    return func(result.Value);
}
```

BindAsyncBoth:

```csharp
internal static class TaskExtensions
{
    Task<T> AsCompletedTask<T>(this T obj) => Task.FromResult(obj);

    ConfiguredTaskAwaitable DefaultAwait(this System.Threading.Tasks.Task task) =>
        task.ConfigureAwait(Result.DefaultConfigureAwait);

    ConfiguredTaskAwaitable<T> DefaultAwait<T>(this Task<T> task) =>
        task.ConfigureAwait(Result.DefaultConfigureAwait);
}
```

```csharp
async Task<Result<TO, TE>> Bind<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task<Result<TO, TE>>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return await result.Bind(func).DefaultAwait();
}
```

BindAsyncLeft:

```csharp
async Task<Result<TO, TE>> Bind<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Result<TO, TE>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Bind(func);
}
```

BindAsyncRight:

```csharp
Task<Result<TO, TE>> Bind<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, Task<Result<TO, TE>>> func)
{
    if (result.IsFailure)
        return Result.Failure<TO, TE>(result.Error).AsCompletedTask();

    return func(result.Value);
}
```

### BindWithTransactionScope

### Check

* If the calling result is a success, the given function is executed and its Result is checked.
* If this Result is a failure, it is returned.
* Otherwise, the calling result is returned

```csharp
Result<TI, TE> Check<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, Result<TO, TE>> func)
{
    return result.Bind(func)
                 .Map(_ => result.Value);
}
```

CheckAsyncBoth:

```csharp
async Task<Result<TI, TE>> Check<TI, TO, TE>(this Task<Result<TI, TE> resultTask,
    Func<TI, Task<Result<TO, TE>>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return await result.Bind(func).Map(_ => result.Value).DefaultAwait();
}
```

CheckAsyncLeft:

```csharp
async Task<Result<TI, TE>> Check<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Result<TO, TE>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Check(func);
}

```

CheckAsyncRight:

```csharp
async Task<Result<TI, TE>> Check<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, Task<Result<TO, TE>>> func)
{
    return await result.Bind(func).Map(_ => result.Value).DefaultAwait();
}

```

### CheckIf

```csharp
Result<TI, TE> CheckIf<TI, TO, TE>(this Result<TI, TE> result,
    bool condition, Func<TI, Result<TO, TE>> func)
{
    if (condition)
        return result.Check(func);
    else
        return result;
}

Result<TI, TE> CheckIf<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, bool> predicate, Func<TI, Result<TO, TE>> func)
{
    if (result.IsSuccess && predicate(result.Value))
        return result.Check(func);
    else
        return result;
}
```

CheckIfAsyncBoth:

```csharp
Task<Result<TI, TE>> CheckIf<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    bool condition, Func<TI, Task<Result<TO, TE>>> func)
{
    if (condition)
        return resultTask.Check(func);
    else
        return resultTask;
}

async Task<Result<TI, TE>> CheckIf<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, Func<TI, Task<Result<TO, TE>>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess && predicate(result.Value))
        return await result.Check(func).DefaultAwait();
    else
        return result;
}
```

CheckIfAsyncLeft:

```csharp
Task<Result<TI, TE>> CheckIf<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    bool condition, Func<TI, Result<TO, TE>> func)
{
    if (condition)
        return resultTask.Check(func);
    else
        return resultTask;
}

async Task<Result<TI, TE>> CheckIf<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, Func<TI, Result<TO, TE>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess && predicate(result.Value))
        return result.Check(func);
    else
        return result;
}
```

CheckIfAsyncRight:

```csharp
Task<Result<TI, TE>> CheckIf<TI, TO, TE>(this Result<TI, TE> result,
    bool condition, Func<TI, Task<Result<TO, TE>>> func)
{
    if (condition)
        return result.Check(func);
    else
        return Task.FromResult(result);
}

Task<Result<TI, TE>> CheckIf<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, bool> predicate, Func<TI, Task<Result<TO, TE>>> func)
{
    if (result.IsSuccess && predicate(result.Value))
        return result.Check(func);
    else
        return Task.FromResult(result);
}
```

### Combine

```csharp
public interface ICombine
{
    ICombine Combine(ICombine value);
}
```

```csharp
Result Combine(this IEnumerable<Result> results, string errorMessageSeparator = null)
    => Result.Combine(results, errorMessageSeparator);

Result<IEnumerable<TI>, TE> Combine<TI, TE>(this IEnumerable<Result<TI, TE>> results)
    where TE : ICombine
{
    results = results.ToList();
    Result<bool, TE> result = Result.Combine(results);

    return result.IsSuccess
        ? Result.Success<IEnumerable<TI>, TE>(results.Select(e => e.Value))
        : Result.Failure<IEnumerable<TI>, TE>(result.Error);
}

Result<IEnumerable<TI>, TE> Combine<TI, TE>(this IEnumerable<Result<TI, TE>> results,
    Func<IEnumerable<TE>, TE> composerError)
{
    results = results.ToList();
    Result<bool, TE> result = Result.Combine(results, composerError);

    return result.IsSuccess
        ? Result.Success<IEnumerable<TI>, TE>(results.Select(e => e.Value))
        : Result.Failure<IEnumerable<TI>, TE>(result.Error);
}

Result<IEnumerable<TI>> Combine<TI>(this IEnumerable<Result<TI>> results,
    string errorMessageSeparator = null)
{
    results = results.ToList();
    Result result = Result.Combine(results, errorMessageSeparator);

    return result.IsSuccess
        ? Result.Success(results.Select(e => e.Value))
        : Result.Failure<IEnumerable<TI>>(result.Error);
}

Result<TO, TE> Combine<TI, TO, TE>(this IEnumerable<Result<TI, TE>> results,
    Func<IEnumerable<TI>, TO> composer, Func<IEnumerable<TE>, TE> composerError)
{
    Result<IEnumerable<TI>, TE> result = results.Combine(composerError);

    return result.IsSuccess
        ? Result.Success<TO, TE>(composer(result.Value))
        : Result.Failure<TO, TE>(result.Error);
}

Result<TO, TE> Combine<TI, TO, TE>(this IEnumerable<Result<TI, TE>> results,
    Func<IEnumerable<TI>, TO> composer)
    where TE : ICombine
{
    Result<IEnumerable<TI>, TE> result = results.Combine<TI, TE>();

    return result.IsSuccess
        ? Result.Success<TO, TE>(composer(result.Value))
        : Result.Failure<TO, TE>(result.Error);
}

Result<TO> Combine<TI, TO>(this IEnumerable<Result<TI>> results,
    Func<IEnumerable<TI>, TO> composer, string errorMessageSeparator = null)
{
    Result<IEnumerable<TI>> result = results.Combine(errorMessageSeparator);

    return result.IsSuccess
        ? Result.Success(composer(result.Value))
        : Result.Failure<TO>(result.Error);
}
```

Есть еще много CombineAsyncLeft.

### Deconstruct

```csharp
void Deconstruct<TI, TE>(this Result<TI, TE> result,
    out bool isSuccess, out bool isFailure, out TI value, out TE error)
{
    isSuccess = result.IsSuccess;
    isFailure = result.IsFailure;
    value = result.IsSuccess ? result.Value : default;
    error = result.IsFailure ? result.Error : default;
}
```

### Ensure

* Returns a new failure result if the predicate is false.
* Otherwise returns the starting result.

```csharp
Result<TI, TE> Ensure<TI, TE>(this Result<TI, TE> result,
    Func<TI, bool> predicate, TE error)
{
    if (result.IsFailure)
        return result;

    if (!predicate(result.Value))
        return Result.Failure<T, E>(error);

    return result;
}

Result<TI, TE> Ensure<TI, TE>(this Result<TI, TE> result,
    Func<TI, bool> predicate, Func<TI, TE> errorPredicate)
{
    if (result.IsFailure)
        return result;

    if (!predicate(result.Value))
        return Result.Failure<TI, TE>(errorPredicate(result.Value));

    return result;
}
```

EnsureAsyncBoth:

```csharp
async Task<Result<TI, TE>> Ensure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task<bool>> predicate, TE error)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return result;

    if (!await predicate(result.Value).DefaultAwait())
        return Result.Failure<TI, TE>(error);

    return result;
}

async Task<Result<TI, TE>> Ensure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task<bool>> predicate, Func<TI, TE> errorPredicate)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return result;

    if (!await predicate(result.Value).DefaultAwait())
        return Result.Failure<TI, TE>(errorPredicate(result.Value));

    return result;
}

async Task<Result<TI, TE>> Ensure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task<bool>> predicate, Func<TI, Task<TE>> errorPredicate)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return result;

    if (!await predicate(result.Value).DefaultAwait())
        return Result.Failure<TI, TE>(await errorPredicate(result.Value));

    return result;
}
```

EnsureAsyncLeft:

```csharp
async Task<Result<TI, TE>> Ensure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, TE error) 
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Ensure(predicate, error);
}

async Task<Result<TI, TE>> Ensure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, Func<TI, TE> errorPredicate)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Ensure(predicate, errorPredicate);
}

async Task<Result<TI>> Ensure<TI>(this Task<Result<TI>> resultTask,
    Func<TI, bool> predicate, Func<TI, string> errorPredicate)
{
    Result<TI> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return result;

    return result.Ensure(predicate, errorPredicate(result.Value));
}

async Task<Result<TI>> Ensure<TI>(this Task<Result<TI>> resultTask,
    Func<TI, bool> predicate, Func<TI, Task<string>> errorPredicate)
{
    Result<TI> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return result;

    return result.Ensure(predicate, await errorPredicate(result.Value));
}
```

EnsureAsyncRight:

```csharp
async Task<Result<TI, TE>> Ensure<TI, TE>(this Result<TI, TE> result,
    Func<TI, Task<bool>> predicate, TE error)
{
    if (result.IsFailure)
        return result;

    if (!await predicate(result.Value).DefaultAwait())
        return Result.Failure<TI, TE>(error);

    return result;
}

async Task<Result<TI, TE>> Ensure<TI, TE>(this Result<TI, TE> result,
    Func<TI, Task<bool>> predicate, Func<TI, TE> errorPredicate)
{
    if (result.IsFailure)
        return result;

    if (!await predicate(result.Value).DefaultAwait())
        return Result.Failure<TI, TE>(errorPredicate(result.Value));

    return result;
}

async Task<Result<TI, TE>> Ensure<TI, TE>(this Result<TI, TE> result,
    Func<TI, Task<bool>> predicate, Func<TI, Task<TE>> errorPredicate)
{
    if (result.IsFailure)
        return result;

    if (!await predicate(result.Value).DefaultAwait())
        return Result.Failure<TI, TE>(await errorPredicate(result.Value));

    return result;
}
```

### Finally

* Passes the result to the given function (regardless of success/failure state)
to yield a final output value.

```csharp
TI Finally<TI>(this Result result, Func<Result, TI> func)
    => func(result);
```

FinallyAsyncBoth:

```csharp
async Task<TO> Finally<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<Result<TI, TE>, Task<TO>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return await func(result).DefaultAwait();
}
```

FinallyAsyncLeft:

```csharp
async Task<TO> Finally<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<Result<TI, TE>, TO> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Finally(func);
}
```

FinallyAsyncRight:

```csharp
async Task<TO> Finally<TI, TO, TE>(this Result<TI, TE> result,
    Func<Result<TI, TE>, Task<TO>> func)
{
    return await func(result).DefaultAwait();
}
```

### Map

* Creates a new result from the return value of a given function.
* If the calling Result is a failure, a new failure result is returned instead.

```csharp
Result<TO, TE> Map<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, TO> func)
{
    if (result.IsFailure)
        return Result.Failure<TO, TE>(result.Error);

    return Result.Success<TO, TE>(func(result.Value));
}
```

MapAsyncBoth:

```csharp
async Task<Result<TO, TE>> Map<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task<TO>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return Result.Failure<TO, TE>(result.Error);

    TO value = await func(result.Value).DefaultAwait();

    return Result.Success<TO, TE>(value);
}
```

MapAsyncLeft:

```csharp
async Task<Result<TO, TE>> Map<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, TO> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Map(func);
}
```

MapAsyncRight:

```csharp
async Task<Result<TO, TE>> Map<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, Task<TO>> func)
{
    if (result.IsFailure)
        return Result.Failure<TO, TE>(result.Error);

    TO value = await func(result.Value).DefaultAwait();

    return Result.Success<TO, TE>(value);
}
```

### MapError

* If the calling Result is a success, a new success result is returned.
* Otherwise, creates a new failure result from the return value of a given function.

```csharp
Result<TI, TE2> MapError<TI, TE, TE2>(this Result<TI, TE> result, Func<TE, TE2> errorFactory)
{
    if (result.IsFailure)
        return Result.Failure<TI, TE2>(errorFactory(result.Error));

    return Result.Success<TI, TE2>(result.Value);
}
```

MapErrorAsyncLeft:

```csharp
async Task<Result<TI, TE2>> MapError<TI, TE, TE2>(this Task<Result<TI, TE>> resultTask,
    Func<TE, TE2> errorFactory)
{
    var result = await resultTask.DefaultAwait();
    return result.MapError(errorFactory);
}
```

### MapWithTransactionScope

### Match

* Returns the result of the given `onSuccess` function if the calling Result is a success.
* Otherwise, it returns the result of the given `onFailure` function.

```csharp
TO Match<TI, TO, TE>(this Result<TI, TE> result, Func<TI, TO> onSuccess, Func<TE, TO> onFailure)
{
    return result.IsSuccess
        ? onSuccess(result.Value)
        : onFailure(result.Error);
}

void Match<TI, TE>(this Result<TI, TE> result, Action<TI> onSuccess, Action<TE> onFailure)
{
    if (result.IsSuccess)
        onSuccess(result.Value);
    else
        onFailure(result.Error);
}
```

MatchAsyncBoth:

```csharp
async Task<TO> Match<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task<TO>> onSuccess, Func<TE, Task<TO>> onFailure)
{
    return await (await resultTask.DefaultAwait())
        .Match(onSuccess, onFailure).DefaultAwait();
}

async Task Match<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task> onSuccess, Func<TE, Task> onFailure)
{
    await (await resultTask.DefaultAwait())
        .Match(onSuccess, onFailure).DefaultAwait();
}

```

MatchAsyncLeft:

```csharp
async Task<TO> Match<TI, TO, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, TO> onSuccess, Func<TE, TO> onFailure)
{
    return (await resultTask.DefaultAwait()).Match(onSuccess, onFailure);
}

async Task Match<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Action<TI> onSuccess, Action<TE> onFailure)
{
    (await resultTask.DefaultAwait()).Match(onSuccess, onFailure);
}
```

MatchAsyncRight:

```csharp
Task<TO> Match<TI, TO, TE>(this Result<TI, TE> result,
    Func<TI, Task<TO>> onSuccess, Func<TE, Task<TO>> onFailure)
{
    return result.IsSuccess
        ? onSuccess(result.Value)
        : onFailure(result.Error);
}

Task Match<TI, TE>(this Result<TI, TE> result,
    Func<TI, Task> onSuccess, Func<TE, Task> onFailure)
{
    return result.IsSuccess
        ? onSuccess(result.Value)
        : onFailure(result.Error);
}
```

### OnFailure

* Executes the given action if the calling result is a failure.
* Returns the calling result.

```csharp
Result<TI, TE> OnFailure<TI, TE>(this Result<TI, TE> result,
    Action action)
{
    if (result.IsFailure)
        action();

    return result;
}

Result<TI, TE> OnFailure<TI, TE>(this Result<TI, TE> result,
    Action<TE> action)
{
    if (result.IsFailure)
        action(result.Error);

    return result;
}
```

OnFailureAsyncBoth:

```csharp
async Task<Result<TI, TE>> OnFailure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<Task> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        await func().DefaultAwait();

    return result;
}

async Task<Result<TI, TE>> OnFailure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TE, Task> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        await func(result.Error).DefaultAwait();

    return result;
}
```

OnFailureAsyncLeft:

```csharp
async Task<Result<TI, TE>> OnFailure<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Action<TE> action)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.OnFailure(action);
}
```

OnFailureAsyncRight:

```csharp
async Task<Result<TI, TE>> OnFailure<TI, TE>(this Result<TI, TE> result, Func<Task> func)
{
    if (result.IsFailure)
        await func().DefaultAwait();

    return result;
}

async Task<Result<TI, TE>> OnFailure<TI, TE>(this Result<TI, TE> result, Func<TE, Task> func)
{
    if (result.IsFailure)
        await func(result.Error).DefaultAwait();

    return result;
}
```

### OnFailureCompensate

```csharp
Result<TI, TE> OnFailureCompensate<TI, TE>(this Result<TI, TE> result,
    Func<Result<TI, TE>> func)
{
    if (result.IsFailure)
        return func();

    return result;
}

Result<TI, TE> OnFailureCompensate<TI, TE>(this Result<TI, TE> result,
    Func<TE, Result<TI, TE>> func)
{
    if (result.IsFailure)
        return func(result.Error);

    return result;
}
```

OnFailureCompensateAsyncBoth:

```csharp
async Task<Result<TI, TE>> OnFailureCompensate<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<Task<Result<TI, TE>>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return await func().DefaultAwait();

    return result;
}

async Task<Result<TI, TE>> OnFailureCompensate<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TE, Task<Result<TI, TE>>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsFailure)
        return await func(result.Error).DefaultAwait();

    return result;
}
```

OnFailureCompensateAsyncLeft:

```csharp
async Task<Result<TI, TE>> OnFailureCompensate<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TE, Result<TI, TE>> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.OnFailureCompensate(func);
}
```

OnFailureCompensateAsyncRight:

```csharp
async Task<Result<TI, TE>> OnFailureCompensate<TI, TE>(this Result<TI, TE> result,
    Func<Task<Result<TI, TE>>> func)
{
    if (result.IsFailure)
        return await func().DefaultAwait();

    return result;
}

async Task<Result<TI, TE>> OnFailureCompensate<TI, TE>(this Result<TI, TE> result,
    Func<TE, Task<Result<TI, TE>>> func)
{
    if (result.IsFailure)
        return await func(result.Error).DefaultAwait();

    return result;
}
```

### OnSuccessTry

```csharp
Result OnSuccessTry<TI>(this Result<TI> result,
    Action<TI> action, Func<Exception, string> errorHandler = null)
{
    return result.IsFailure
        ? Result.Failure(result.Error)
        : Result.Try(() => action(result.Value), errorHandler);
}

Result<TO> OnSuccessTry<TI, TO>(this Result<TI> result,
    Func<TI, TO> func, Func<Exception, string> errorHandler = null)
{
    return result.IsFailure
        ? Result.Failure<TO>(result.Error)
        : Result.Try(() => func(result.Value), errorHandler);
}
```

OnSuccessTryAsyncBoth:

```csharp
async Task<Result<TI>> OnSuccessTry<TI>(this Task<Result> task,
    Func<Task<TI>> func, Func<Exception, string> errorHandler = null)
{
    var result = await task.DefaultAwait();
    return await result.OnSuccessTry(func, errorHandler);
}

async Task<Result> OnSuccessTry<TI>(this Task<Result<TI>> task,
    Func<TI, Task> func, Func<Exception, string> errorHandler = null)
{
    var result = await task.DefaultAwait();
    return await result.OnSuccessTry(func, errorHandler);
}

async Task<Result<TO>> OnSuccessTry<TI, TO>(this Task<Result<TI>> task,
    Func<TI, Task<TO>> func, Func<Exception, string> errorHandler = null)
{
    var result = await task.DefaultAwait();
    return await result.OnSuccessTry(func, errorHandler);
}
```

OnSuccessTryAsyncLeft:

```csharp
async Task<Result<TI>> OnSuccessTry<TI>(this Task<Result> task,
    Func<TI> func, Func<Exception, string> errorHandler = null)
{
    var result = await task.DefaultAwait();
    return result.OnSuccessTry(func, errorHandler);
}

async Task<Result> OnSuccessTry<TI>(this Task<Result<TI>> task,
    Action<TI> action, Func<Exception, string> errorHandler = null)
{
    var result = await task.DefaultAwait();
    return result.OnSuccessTry(action, errorHandler);
}

async Task<Result<TO>> OnSuccessTry<TI, TO>(this Task<Result<TI>> task,
    Func<TI, TO> action, Func<Exception, string> errorHandler = null)
{
    var result = await task.DefaultAwait();
    return result.OnSuccessTry(action, errorHandler);
}
```

OnSuccessTryAsyncRight:

```csharp
async Task OnSuccessTry<TI, TE>(this Result<TI, TE> result,
    Func<TI, Task> func)
{
    if (result.IsSuccess)
        await func(result.Value);
}

async Task<Result<TI>> OnSuccessTry<TI>(this Result result,
    Func<Task<TI>> func, Func<Exception, string> errorHandler = null)
{
    return result.IsFailure
       ? Result.Failure<TI>(result.Error)
       : await Result.Try(func, errorHandler);
}

async Task<Result> OnSuccessTry<TI>(this Result<TI> result,
    Func<TI, Task> func, Func<Exception, string> errorHandler = null)
{
    return result.IsFailure
       ? Result.Failure(result.Error)
       : await Result.Try(async () => await func(result.Value), errorHandler);
}

async Task<Result<TO>> OnSuccessTry<TI, TO>(this Result<TI> result,
    Func<TI, Task<TO>> func, Func<Exception, string> errorHandler = null)
{
    return result.IsFailure
       ? Result.Failure<TO>(result.Error)
       : await Result.Try(async () => await func(result.Value), errorHandler);
}
```

### SelectMany

* This method should be used in linq queries. We recommend using Bind method.

### Tap

* Executes the given action if the calling result is a success.
* Returns the calling result.

```csharp
Result<TI, TE> Tap<TI, TE>(this Result<TI, TE> result,
    Action action)
{
    if (result.IsSuccess)
        action();

    return result;
}

Result<TI, TE> Tap<TI, TE>(this Result<TI, TE> result,
    Action<TI> action)
{
    if (result.IsSuccess)
        action(result.Value);

    return result;
}
```

TapAsyncBoth:

```csharp
async Task<Result<TI, TE>> Tap<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<Task> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess)
        await func().DefaultAwait();

    return result;
}

async Task<Result<TI, TE>> Tap<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, Task> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess)
        await func(result.Value).DefaultAwait();

    return result;
}
```

TapAsyncLeft:

```csharp
async Task<Result<TI, TE>> Tap<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Action action)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Tap(action);
}

async Task<Result<TI, TE>> Tap<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Action<TI> action)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();
    return result.Tap(action);
}
```

TapAsyncRight:

```csharp
async Task<Result<TI, TE>> Tap<TI, TE>(this Result<TI, TE> result,
    Func<Task> func)
{
    if (result.IsSuccess)
        await func().DefaultAwait();

    return result;
}

async Task<Result<TI, TE>> Tap<TI, TE>(this Result<TI, TE> result,
    Func<TI, Task> func)
{
    if (result.IsSuccess)
        await func(result.Value).DefaultAwait();

    return result;
}
```

### TapIf

* Executes the given action if the calling result is a success and condition is true.
* Returns the calling result.

```csharp
Result<TI, TE> TapIf<TI, TE>(this Result<TI, TE> result,
    bool condition, Action action)
{
    if (condition)
        return result.Tap(action);
    else
        return result;
}

Result<TI, TE> TapIf<TI, TE>(this Result<TI, TE> result,
    bool condition, Action<TI> action)
{
    if (condition)
        return result.Tap(action);
    else
        return result;
}

Result<TI, TE> TapIf<TI, TE>(this Result<TI, TE> result,
    Func<TI, bool> predicate, Action action)
{
    if (result.IsSuccess && predicate(result.Value))
        return result.Tap(action);
    else
        return result;
}

Result<TI, TE> TapIf<TI, TE>(this Result<TI, TE> result,
    Func<TI, bool> predicate, Action<TI> action)
{
    if (result.IsSuccess && predicate(result.Value))
        return result.Tap(action);
    else
        return result;
}
```

TapIfAsyncBoth:

```csharp
Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    bool condition, Func<Task> func)
{
    if (condition)
        return resultTask.Tap(func);
    else
        return resultTask;
}

Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    bool condition, Func<TI, Task> func)
{
    if (condition)
        return resultTask.Tap(func);
    else
        return resultTask;
}

async Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, Func<Task> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess && predicate(result.Value))
        return await result.Tap(func).DefaultAwait();
    else
        return result;
}

async Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, Func<TI, Task> func)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess && predicate(result.Value))
        return await result.Tap(func).DefaultAwait();
    else
        return result;
}
```

TapIfAsyncLeft:

```csharp
Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    bool condition, Action action)
{
    if (condition)
        return resultTask.Tap(action);
    else
        return resultTask;
}

Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    bool condition, Action<TI> action)
{
    if (condition)
        return resultTask.Tap(action);
    else
        return resultTask;
}

async Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, Action action)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess && predicate(result.Value))
        return result.Tap(action);
    else
        return result;
}

async Task<Result<TI, TE>> TapIf<TI, TE>(this Task<Result<TI, TE>> resultTask,
    Func<TI, bool> predicate, Action<TI> action)
{
    Result<TI, TE> result = await resultTask.DefaultAwait();

    if (result.IsSuccess && predicate(result.Value))
        return result.Tap(action);
    else
        return result;
}
```

TapIfAsyncRight:

If !NET40

### WithTransactionScope

## Methods

### Combine

* Combines several results (and any error messages) into a single result.
* The returned result will be a failure if any of the input `results` are failures.

```csharp
Result Combine<TI>(
    IEnumerable<Result<TI>> results, string errorMessagesSeparator = null)
{
    IEnumerable<Result> untyped = results.Select(result => (Result)result);
    return Combine(untyped, errorMessagesSeparator);
}

Result<bool, TE> Combine<T, TE>(
    IEnumerable<Result<T, TE>> results) where TE : ICombine
{
    return Combine(results, (errors) => errors.Aggregate((x, y) => (TE)x.Combine(y)));
}

Result<bool, TE> Combine<TI, TE>(
    IEnumerable<Result<TI, TE>> results, Func<IEnumerable<TE>, TE> composerError)
{
    List<Result<TI, TE>> failedResults = results.Where(x => x.IsFailure).ToList();

    if (failedResults.Count == 0)
        return Success<bool, TE>(true);

    TE error = composerError(failedResults.Select(x => x.Error));
        return Failure<bool, TE>(error);
}

Result Combine<TI>(params Result<TI>[] results)
{
    return Combine(results, ErrorMessagesSeparator);
}

Result<bool, TE> Combine<T, TE>(params Result<T, TE>[] results)
    where TE : ICombine
{
    return Combine(results, (errors) => errors.Aggregate((x, y) => (TE)x.Combine(y)));
}

Result Combine<TI>(string errorMessagesSeparator, params Result<TI>[] results)
{
    return Combine(results, errorMessagesSeparator);
}

Result<bool, TE> Combine<TI, TE>(
    Func<IEnumerable<TE>, TE> composerError, params Result<TI, TE>[] results)
{
    return Combine(results, composerError);
}

Result Combine(IEnumerable<Result> results, string errorMessagesSeparator = null)
{
    List<Result> failedResults = results.Where(x => x.IsFailure).ToList();

    if (failedResults.Count == 0)
        return Success();

    string errorMessage = string.Join(errorMessagesSeparator
         ?? ErrorMessagesSeparator, AggregateMessages(failedResults.Select(x => x.Error)));

    return Failure(errorMessage);
}

private static IEnumerable<string> AggregateMessages(IEnumerable<string> messages)
{
    var dict = new Dictionary<string, int>();
    foreach (var message in messages)
    {
        if (!dict.ContainsKey(message))
            dict.Add(message, 0);

        dict[message]++;
    }

    return dict.Select(x => x.Value == 1 ? x.Key : $"{x.Key} ({x.Value}×)");
}
```

### ConvertFailure

* Throws `InvalidOperationException` if the result is a success.
* Else returns a new failure result of the given type.

```csharp
public Result<TO, TE> ConvertFailure<TO>()
{
    if (IsSuccess)
        throw new InvalidOperationException(Result.Messages.ConvertFailureExceptionOnSuccess);

    return Result.Failure<TO, TE>(Error);
}
```

### Failure

* Creates a failure result with the given error message.

```csharp
Result<TI, TE> Failure<TI, TE>(TE error)
{
    return new Result<TI, TE>(true, error, default);
}
```

### FailureIf

* Creates a result whose success/failure reflects the supplied condition.
* Creates a result whose success/failure depends on the supplied predicate.
* Opposite of SuccessIf().

```csharp
Result<TI, TE> FailureIf<TI, TE>(
    bool isFailure, TI value, TE error)
{
    return SuccessIf(!isFailure, value, error);
}

Result<TI, TE> FailureIf<TI, TE>(
    Func<bool> failurePredicate, TI value, TE error)
{
    return SuccessIf(!failurePredicate(), value, error);
}

async Task<Result<TI, TE>> FailureIf<TI, TE>(
    Func<Task<bool>> failurePredicate, TI value, TE error)
{
    bool isFailure = await failurePredicate().DefaultAwait();
    return SuccessIf(!isFailure, value, error);
}
```

### FirstFailureOrSuccess

* Returns the first failure from the supplied `results`.
* If there is no failure, a success result is returned.

```csharp
Result FirstFailureOrSuccess(params Result[] results)
{
    foreach (Result result in results)
    {
        if (result.IsFailure)
            return result;
    }

    return Success();
}
```

### Success

* Creates a success result containing the given value.

```csharp
Result<TI, TE> Success<TI, TE>(TI value)
{
    return new Result<TI, TE>(false, default, value);
}
```

### SuccessIf

* Creates a result whose success/failure reflects the supplied condition.
* Creates a result whose success/failure depends on the supplied predicate.
* Opposite of FailureIf().

```csharp
Result<TI, TE> SuccessIf<TI, TE>(
    bool isSuccess, TI value, TE error)
{
    return isSuccess
        ? Success<TI, TE>(value)
        : Failure<TI, TE>(error);
}

Result<TI, TE> SuccessIf<TI, TE>(
    Func<bool> predicate, TI value, TE error)
{
    return SuccessIf(predicate(), value, error);
}

async Task<Result<TI, TE>> SuccessIf<TI, TE>(
    Func<Task<bool>> predicate, TI value, TE error)
{
    bool isSuccess = await predicate().DefaultAwait();
    return SuccessIf(isSuccess, value, error);
}
```

### Try

* Attempts to execute the supplied action.
* Returns a Result indicating whether the action executed successfully.
* If the function executed successfully, the result contains its return value.

```csharp
Result<TI, TE> Try<TI, TE>(
    Func<TI> func, Func<Exception, TE> errorHandler)
{
    try
    {
        return Success<TI, TE>(func());
    }
    catch (Exception exc)
    {
        TE error = errorHandler(exc);
        return Failure<TI, TE>(error);
    }
}

async Task<Result<TI, TE>> Try<TI, TE>(
    Func<Task<TI>> func, Func<Exception, TE> errorHandler)
{
    try
    {
        var result = await func().DefaultAwait();
        return Success<TI, TE>(result);
    }
    catch (Exception exc)
    {
        TE error = errorHandler(exc);
        return Failure<TI, TE>(error);
    }
}
```