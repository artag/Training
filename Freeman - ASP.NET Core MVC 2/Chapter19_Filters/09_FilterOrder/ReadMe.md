# Порядок применения фильтров и его изменение

Фильтры запускаются в специфической очередности:
* Фильтры авторизации
* Фильтры действий
* Фильтры результатов

При наличии нескольких фильтров заданного типа порядок их применения управляется областью действия,
через которую фильтры были применены.

В примере задан фильтр `Infrastructure/MessageAttribute`, который выводит сообщения до и после
обработки результата действия:
```cs
public class MessageAttribute : ResultFilterAttribute
{
    private readonly string _message;

    public MessageAttribute(string message)
    {
        _message = message;
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var controller = context.Controller as Controller;
        var key = $"Before Result: {_message}";
        controller.ViewData[key] = string.Empty;
    }

    public override void OnResultExecuted(ResultExecutedContext context)
    {
        var message = $"After Result: {_message}";
        var bytes = Encoding.ASCII.GetBytes($"<div>{message}</div>");
        context.HttpContext.Response.Body.Write(bytes, 0, bytes.Length);
    }
}
```

Сообщение фильтра конфигурируется посредством аргумента конструктора.

Последовательность вызова аттрибута `Message` приведена в `Controllers/DefaultOrderController`:
```cs
[Message("This is the Controller-Scoped Filter")]
public class DefaultOrderController : Controller
{
    [Message("This is the First Action-Scoped Filter")]
    [Message("This is the Second Action-Scoped Filter")]
    public IActionResult Index() =>
        View("Message", "This is default order controller");
}
```

Плюс, `Message` был зарегистрирован как глобальный фильтр (см. Startup.ConfigureServices()):
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().AddMvcOptions(options =>
    {
        options.Filters.Add(new MessageAttribute("This is the Globally-Scoped Filter"));
    });
}
```

При запуске будет наблюдаться следующий вывод:
```
Before Result: This is the Globally-Scoped Filter
Before Result: This is the Controller-Scoped Filter
Before Result: This is the First Action-Scoped Filter
Before Result: This is the Second Action-Scoped Filter
After Result: This is the Second Action-Scoped Filter
After Result: This is the First Action-Scoped Filter
After Result: This is the Controller-Scoped Filter
After Result: This is the Globally-Scoped Filter
```

Порядок запуска фильтров по умолчанию:
1. Глобальные фильтры.
2. Фильтры, примененные к классу контроллера.
3. Фильтры, примененные к методам действий.


## Изменение порядка применения фильтров

Стандартный порядок может быть изменен за счет реализации интерфейса `IOrderedFilter`:
```cs
public interface IOrderedFilter : IFilterMetadata
{
    int Order { get; }
}
```

Низкое значение свойства `Order` указывает MVC на необходимость применения фильтра перед фильтрами
с более высокими значениями `Order`.

Атрибуты уже реализуют интерфейс `IOrderedFilter`.

Пример (из `Controllers/CustomOrderController`):
```cs
[Message("This is the Controller-Scoped Filter", Order = 10)]
public class CustomOrderController : Controller
{
    [Message("This is the First Action-Scoped Filter", Order = 1)]
    [Message("This is the Second Action-Scoped Filter", Order = -1)]
    public IActionResult Index() =>
        View("Message", "This is custom order controller");
}
```

А вывод будет такой:
```
Before Result: This is the Second Action-Scoped Filter
Before Result: This is the Globally-Scoped Filter
Before Result: This is the First Action-Scoped Filter
Before Result: This is the Controller-Scoped Filter
After Result: This is the Controller-Scoped Filter
After Result: This is the First Action-Scoped Filter
After Result: This is the Globally-Scoped Filter
After Result: This is the Second Action-Scoped Filter
```
