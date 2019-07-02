# Создание глобальных фильтров

Глобальные фильтры применяются один раз, в классе `Startup`, а потом автоматически применяются
к каждому методу действия в каждом контроллере внутри приложения.

В качестве глобального может использоваться любой фильтр.

Пример первого фильтра (см. `Infrastructure/ViewResultDiagnostics`), который используется в
качестве глобального:
```cs
public class ViewResultDiagnostics : IActionFilter
{
    private readonly IFilterDiagnostics _diagnostics;

    public ViewResultDiagnostics(IFilterDiagnostics diagnostics)
    {
        _diagnostics = diagnostics;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        ViewResult vr;
        if ((vr = context.Result as ViewResult) == null)
        {
            return;
        }

        _diagnostics.AddMessage($"View name: {vr.ViewName}");
        _diagnostics.AddMessage($"Model type: {vr.ViewData.Model.GetType().Name}");
    }
}
```

Фильтр использует объект `IFilterDiagnostics` для сохранения сообщений об имени представления и
типе модели результатов.

Второй фильтр, который используется в качестве глобального (см. `Infrastructure/DiagnosticsFilter`):
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

Фильтр использует объект `IFilterDiagnostics` для записи диагностических сообщений в ответ клиенту.

Глобальные фильтры настраиваются путем конфигурирования служб MVC
(см. `Startup/ConfigureServices()`):
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<DiagnosticsFilter>();
    services.AddScoped<ViewResultDiagnostics>();
    services.AddScoped<IFilterDiagnostics, DefaultFilterDiagnostics>();
    services.AddMvc().AddMvcOptions(options =>
    {
        options.Filters.AddService(typeof(ViewResultDiagnostics));
        options.Filters.AddService(typeof(DiagnosticsFilter));
    });
}
```

Для регистрации фильтров как глобальных используется `MvcOptions.Filters.AddService()`.
`AddService` принимает тип .NET экземпляр которого будет создан с применением правил жизненного
цикла, указанного в `ConfigureServices()`.

Если посмотреть на `HomeController`, то никакие фильтры при его вызове не применяются, тем не
менее, к выводу контроллера добавлены диагностические строки (работа глобальных фильтров).
