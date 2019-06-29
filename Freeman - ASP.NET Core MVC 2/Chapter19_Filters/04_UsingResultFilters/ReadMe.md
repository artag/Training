# Использование фильтров результатов

Применяются до и после того, как MVC обработала результат, возвращаемый методом действия.
Фильтры результатов могут изменить или заменить результат действия, даже полностью аннулировать
запрос.

Интерфейс `IResultFilter`:
```cs
public interface IResultFilter : IFilterMetadata
{
    void OnResultExecuting(ResultExecutingContext context);
    void OnResultExecuted(ResultExecutedContext context);
}
```

`OnResultExecuting()` вызывается до того, как результат действия, выпущенный методом действия
будет обработан и снабжается информацией контекста через `ResultExecutingContext`
(производный от `FilterContext`).


Свойства `ResultExecutingContext`:
* `Controller` - возвращает объект контроллера, чей метод действия был выполнен.

* `Cancel` - тип `bool`, установка в `true` остановит обработку результата действия для генерации
ответа.

* `Result` - возвращает `IActionResult`, возвращаемый методом действия.


`OnResultExecuted()` вызывается после обработки MVC результата действия и снабжается информацией
контекста через `ResultExecutedContext` (производный от `FilterContext`).

Свойства `ResultExecutedContext`:
* `Controller` - возвращает объект контроллера, чей метод действия был выполнен.

* `Canceled` - тип `bool`, указывает был ли аннулирован запрос.

* `Exception` - содержит любой объект `Exception`, который был сгенерирован методом действия.

* `ExceptionDispatchInfo` - возвращает `ExceptionDispatchInfo`, который содержит информацию
трассировки стека для любого исключения, сгенерированного методом действия.

* `ExceptionHandled` - установка в `true` указывает, что фильтр обработал исклчение, которое
дальше распространяться не будет.

* `Result` - возвращает `IActionResult`, который используется для генерации ответа клиенту.


## Создание фильтра результатов

Самый простой способ создания фильтра результатов - использовать `ResultFilterAttribute`.
См. `Infrastructure/ViewResultDetailsAttribute`:
```cs
public class ViewResultDetailsAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var dict = new Dictionary<string, string>();
        dict["Result Type"] = context.Result.GetType().Name;

        ViewResult vr;
        if ((vr = context.Result as ViewResult) != null)
        {
            dict["View Name"] = vr.ViewName;
            dict["Model Type"] = vr.ViewData.Model.GetType().Name;
            dict["Model Data"] = vr.ViewData.Model.ToString();
        }

        context.Result = new ViewResult
        {
            ViewName = "Message",
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = dict
            }
        };
    }
}
```

Здесь переопределяется `OnResultExecuting()` и используется объект контекста для изменения
результата действия, применяемого при генерации ответа клиенту.

Фильтр создает объект `ViewResult`, который передается во View `Message` в качестве модели
в виде словаря.

Изменение свойства `Result` у контекста позволяет предоставить другой тип результата действия,
отличный от результата действия контроллера.


## Создание асинхронного фильтра результатов

Интерфейс `IAsyncResultFilter`:
```cs
public interface IAsyncResultFilter : IFilterMetadata
{
    Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next);
}
```

Создание (из `Infrastructure/ViewResultDetailsAsyncAttribute`):
```cs
public class ViewResultDetailsAsyncAttribute : ResultFilterAttribute
{
    public override async Task OnResultExecutionAsync(
        ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var dict = new Dictionary<string, string>();
        dict["Result Type"] = context.Result.GetType().Name;

        ViewResult vr;
        if ((vr = context.Result as ViewResult) != null)
        {
            dict["View Name"] = vr.ViewName;
            dict["Model Type"] = vr.ViewData.Model.GetType().Name;
            dict["Model Data"] = vr.ViewData.Model.ToString();
        }

        context.Result = new ViewResult
        {
            ViewName = "Message",
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = dict
            }
        };

        await next();
    }
}
```

Если не вызвать делегат `next()`, то конвейер обработки запросов не будет завершен и результат
действия не визуализируется.

В остальном, все действует здесь аналогично предыдущему примеру фильтра.


## Создание гибридного фильтра действий/результатов

Можно создать фильтр, который одновременно является фильтром и действий и результатов.

Можно реализовать, используя в качестве базового класс `ActionFilterAttribute`.

Пример создания можно посмотреть в `Infrastructure/ProfileAttribute`:
```cs
public class ProfileAttribute : ActionFilterAttribute
{
    private Stopwatch _timer;
    private double _actionTime;

    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _timer = Stopwatch.StartNew();

        await next();

        _actionTime = _timer.Elapsed.TotalMilliseconds;
    }

    public override async Task OnResultExecutionAsync(
        ResultExecutingContext context, ResultExecutionDelegate next)
    {
        _timer.Stop();

        var actionTime = $"{_actionTime} ms";
        var elapsedTime = $"{_timer.Elapsed.TotalMilliseconds} ms";

        var controller = context.Controller as HomeController;
        controller.ViewData["Action Time"] = actionTime;
        controller.ViewData["Elapsed Time"] = elapsedTime;

        await next();
    }
}
```
