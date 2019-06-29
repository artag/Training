# Использование внедрения зависимостей для фильтров

Для обработки каждого запроса MVC создает новый экземпляр класса фильтра.

Альтернативный подход: применение системы внедрения зависимостей с целью выбора другого жизненного
цикла для фильтров. Два похода для использования внедрения зависимостей.


## Распознавание зависимостей в фильтрах

Первый подход для использования внедрения зависимостей в применении внедрения зависимостей для
управления данными контекста, использующимися в фильтрах.

В качестве внедряемого интерфейса используется `IFilterDiagnostics`
и его реализация `DefaultFilterDiagnostics`.

Конфигурация поставщика службы (из `Startup.ConfigureServices`):
```cs
...
services.AddScoped<IFilterDiagnostics, DefaultFilterDiagnostics>();
...
```

Все фильтры, создаваемые для работы с одиночным запросом будут получать один и тот же
`DefaultFilterDiagnostics`.


## Создание фильтров с зависимостями

Класс `TimeFilter` (`Infrastructure/TimeFilter`) это гибридный фильтр действий/результатов,
использует `IFilterDiagnostics` для хранения результатов:
```cs
public class TimeFilter : IAsyncActionFilter, IAsyncResultFilter
{
    private Stopwatch _timer;
    private IFilterDiagnostics _diagnostics;

    public TimeFilter(IFilterDiagnostics diagnostics)
    {
        _diagnostics = diagnostics;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _timer = Stopwatch.StartNew();
        await next();
        _diagnostics.AddMessage($"Action time: {_timer.Elapsed.TotalMilliseconds} ms");
    }

    public async Task OnResultExecutionAsync(
        ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await next();
        _timer.Stop();
        _diagnostics.AddMessage($"Result time: {_timer.Elapsed.TotalMilliseconds} ms");
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

## Применение фильтров

Так как фильтры не являются атрибутами (из-за внедрения зависимостей), то для их использования
применяется атрибут `TypeFilter`. Порядок применения фильтров важен (примеры см. далее в этой главе).

Из `Controllers/HomeController`:
```cs
[TypeFilter(typeof(DiagnosticsFilter))]
[TypeFilter(typeof(TimeFilter))]
public class HomeController : Controller
{
    public IActionResult Index() =>
        View("Message", "This is the Index action on the Home controller");
}
```

Атрибут `TypeFilter` создает новый экземпляры фильтров для каждого запроса, но делает это с
использованием средства внедрения зависимостей.
