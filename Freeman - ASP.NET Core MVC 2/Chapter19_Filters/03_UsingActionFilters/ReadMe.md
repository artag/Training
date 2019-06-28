# Использование фильтров действий

Интерфейс `IActionFilter`:
```cs
public interface IActionFilter : IFilterMetadata
{
    void OnActionExecuting(ActionExecutingContext context);
    void OnActionExecuted(ActionExecutedContext context);
}
```

`OnActionExecuting()` вызывается перед вызовом метода действия, `OnActionExecuted()` - сразу после
вызова метода действия. Соответственно для каждого из методов используются данные контекста:
`ActionExecutingContext` и `ActionExecutedContext`. Оба класса расширяют `FilterContext`.

Свойства `FilterContext`:
* `ActionDescriptor`- Объект `ActionDescriptor`. Описывает метод действия.

* `HttpContext`- Объект `HttpContext`. Предоставляет детали HTTP-запроса и HTTP-ответа.

* `ModelState`- Объект `ModelStateDictionary`. Используется для проверки достоверности данных,
отправленных клиентом.

* `RouteData`- Объект `RouteData`. Описывает способ обработки запроса системой маршрутизации.

* `Filters`- Объект `IList<IFilterMetadata>`. Список фильтров, которые были применены к методу
действия.


Дополнительные свойства `ActionExecutingContext`:
* `Controller` - объект контроллера, методы действий которого должны быть вызваны.
Детали действия доступны через свойство `ActionDescriptor`, унаследованное от базовых классов.

* `ActionArguments` - словарь аргументов, которые будут переданы методу действия.
Фильтр может добавлять, удалять либо изменять аргументы.

* `Result` - если фильтр присвоит этому свойству объект `IActionResult`, тогда произойдет обход
процесса процесса обработки запросов и результат действия будет применяться для генерации ответа
клиенту без обращения к методу действия.


Дополнительные свойства `ActionExecutedContext`:
* `Controller` - объект контроллера, чей метод действия будет вызваны.

`Canceled` - тип `bool`, устанавливается в `true`, если другой фильтр действий предпринял обход
процесса обработки запросов за счет присвоения результата действия свойству `Result` объекта
`ActionExecutingContext`.

`Exception` - содержит любой объект `Exception`, который был сгенерирован методом действия.

`ExceptionDispatchInfo` - объект `ExceptionDispatchInfo`, содержит информацию трассировки стека для
любого исключения, сгенерированного методом действия.

`ExceptionHandled` - установка свойства в `true` указывает, что фильтр обработал исключение, которое
дальше распространяться не будет.

`Result` - возвращает объект реализации `IActionResult`, возвращенный методом действия. При
необходимости фильтр может изменить или полностью заменить результат действия.


## Создание фильтра действия

Фильтры действия используются для:
* Прерывания обработки запроса перед вызовом действия.
* Для изменения результата после выполнения действия.

Простейший способ создания - наследование от `ActionFilterAttribute`, который реализует `IActionFilter`.

Пример см. в `Infrastructure/ProfileAttribute`:
```cs
public class ProfileAttribute : ActionFilterAttribute
{
    private Stopwatch _timer;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _timer = Stopwatch.StartNew();
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _timer.Stop();

        var result = $"{_timer.Elapsed.TotalMilliseconds} ms";
        var controller = context.Controller as HomeController;
        controller.ViewData["ElapsedTime"] = result;
    }
}
```

Таймер запускает в `OnActionExecuting` и останавливается в `OnActionExecuted`.


## Создание асинхронного фильтра действий

Интерфейс `IAsyncActionFilter`:
```cs
public interface IAsyncActionFilter : IFilterMetadata
{
    Task OnActionExecutionAsync(ActionExecutingContext context, ActionExectionDelegate next);
}
```

Единственный метод, посредством продолжения задачи позволяет запускаться фильтру до и после
выполнения метода действия.

Пример см. в `Infrastructure/ProfileAttribute`:
```cs
public override async Task OnActionExecutionAsync(
    ActionExecutingContext context, ActionExecutionDelegate next)
{
    var timer = Stopwatch.StartNew();

    await next();

    timer.Stop();

    var result = $"{timer.Elapsed.TotalMilliseconds} ms";
    var controller = context.Controller as HomeController;
    controller.ViewData["ElapsedTime"] = result;
}
```

`ActionExecutingContext()` снабжает фильтр данными контекста, объект `ActionExecutionDelegate`
представляет метод действия (или следующий фильтр), послежащий выполнению.
Фильтр выполняет свою подготовительную работу перед вызовом делегата и затем завершает работу,
когда делегат заканчивает функционирование. Делегат возвращает объект `Task`, поэтому в листинге
применяется `await`.
