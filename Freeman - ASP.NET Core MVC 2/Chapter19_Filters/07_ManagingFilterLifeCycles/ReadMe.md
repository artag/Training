# Управление жизненными циклами фильтров

Атрибут `ServiceFilter` применяет поставщик служб для создания объекта фильтра, помещая его
под управление жизненным циклом.


## Создание фильтров с жизненными циклами

В качестве внедряемого интерфейса используется `IFilterDiagnostics`
и его реализация `DefaultFilterDiagnostics`.

Класс `TimeFilter` (`Infrastructure/TimeFilter`) это гибридный фильтр действий/результатов,
использует `IFilterDiagnostics` для хранения результатов. Плюс, он сохраняет
среднее арифметическое записанных значений времени.
```cs
public class TimeFilter : IAsyncActionFilter, IAsyncResultFilter
{
    private readonly IFilterDiagnostics _diagnostics;

    private ConcurrentQueue<double> _actionTimes = new ConcurrentQueue<double>();
    private ConcurrentQueue<double> _resultTimes = new ConcurrentQueue<double>();

    public TimeFilter(IFilterDiagnostics diagnostics)
    {
        _diagnostics = diagnostics;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var timer = Stopwatch.StartNew();

        await next();

        timer.Stop();

        _actionTimes.Enqueue(timer.Elapsed.TotalMilliseconds);
        _diagnostics.AddMessage(
            $"Action time: {timer.Elapsed.TotalMilliseconds} ms. " +
            $"Average: {_actionTimes.Average():F2}");
    }

    public async Task OnResultExecutionAsync(
        ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var timer = Stopwatch.StartNew();
        await next();
        timer.Stop();

        _resultTimes.Enqueue(timer.Elapsed.TotalMilliseconds);
        _diagnostics.AddMessage(
            $"Result time: {timer.Elapsed.TotalMilliseconds} ms. " +
            $"Average: {_resultTimes.Average():F2}");
    }
}
```

Класс `DiagnosticsFilter` (`Infrastructure/DiagnosticsFilter`) использует `IFilterDiagnostics` -
записывает содержащиеся в нем сообщения в ответ:
```cs
public class DiagnosticsFilter : IAsyncResultFilter
{
    private readonly IFilterDiagnostics _diagnostics;

    public DiagnosticsFilter(IFilterDiagnostics diagnostics)
    {
        _diagnostics = diagnostics;
    }

    public async Task OnResultExecutionAsync(
        ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await next();

        foreach (var message in _diagnostics?.Messages)
        {
            var bytes = Encoding.ASCII.GetBytes($"<div>{message}</div>");
            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
```

В поставщике служб `TimeFilter` и `DefaultFilterDiagnostics` регистрируются как одиночки, чтобы
заново не создаваться для каждого запроса.
(см. `Startup.ConfigureServices()`):
```cs
services.AddSingleton<TimeFilter>();
services.AddSingleton<IFilterDiagnostics, DefaultFilterDiagnostics>();
```


## Применение фильтра

Для использования `DiagnosticsFilter` применяется атрибут `TypeFilter` (чтобы разрешать зависимости).
Для `TimeFilter` применяется атрибут `ServiceType`. Из (`Controllers/HomeController`):
```cs
[TypeFilter(typeof(DiagnosticsFilter))]
[ServiceFilter(typeof(TimeFilter))]
public class HomeController : Controller
{
    public IActionResult Index() =>
        View("Message", "This is the Index action on the Home controller");
}
```

Примечание: порядок применения фильтров важен.
