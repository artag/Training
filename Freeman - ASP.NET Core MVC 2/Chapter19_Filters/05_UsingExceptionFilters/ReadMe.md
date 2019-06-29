# Использование фильтров исключений

Позволяют реагировать на исключения без необходимости в написании блоков `try...catch`.

Реализуют интерфейс `IExceptionFilter` или `IAsyncExceptionFilter`:
```cs
public interface IExceptionFilter : IFilterMetadata
{
    void OnException(ExceptionContext context);
}

public interface IAsyncExceptionFilter : IFilterMetadata
{
    Task OnExceptionAsync(ExceptionContext context);
}
```

`OnException()` или `OnExceptionAsync()` вызываются при наличии необработанного исключения.

Данные контекста `ExceptionContext`, производные от `FilterContext`.

Дополнительные свойства `ExceptionContext`:
* `Exception` - содержит любой объект `Exception`, который был сгенерирован.

* `ExceptionDispatchInfo` - возвращает `ExceptionDispatchInfo`, содержит информацию трассировки
стека для исключения.

* `ExceptionHandled` - тип `bool`. Используется для указания, обработано ли исключение.

* `Result` - устанавливает `IActionResult`, который будет применяться для ответа.


## Создание фильтра исключений

Наиболее распространенное использование фильтра исключений - отображение специальной страницы
ошибки для специфического типа исключения (вместо использования стандартной обработки ошибок).

Класс `ExceptionFilterAttribute` используется в качестве базового.
Пример в `Infrastructure/RangeExceptionAttribute`:
```cs
public class RangeExceptionAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (!(context.Exception is ArgumentOutOfRangeException))
        {
            return;
        }

        context.Result = new ViewResult
        {
            ViewName = "Message",
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = @"The data received by the application cannot be processed"
            }
        };
    }
}
```

В данном примере фильтр реагирует на исключение `ArgumentOutOfRangeException`: создается
результат действия, который отображает сообщение пользователю.

Тестирование:
* `https://localhost:5001/Home/GenerateException/7` - все OK.

* `https://localhost:5001/Home/GenerateException` - исключение `ArgumentNullException`,
задействуется стандартная обработка ошибок.

`https://localhost:5001/Home/GenerateException/42` - сработает фильтр исключений.
