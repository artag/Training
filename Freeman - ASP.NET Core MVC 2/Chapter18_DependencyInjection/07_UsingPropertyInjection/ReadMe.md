# Использование атрибутов внедрения в свойства

В случае наследования контроллера от базового класса `Controller` использовать нижеприведенные
атрибуты не обязательно.

Специализированные атрибуты для внедрения в свойства:

* `ControllerContext` - устанавливает свойство `ControllerContext`, которое предоставляет
надмножество функциональности класса `ActionContext` (см. главу 31).

* `ActionContext` -  устанавливает свойство `ActionContext` для предоставления конекста методам
действий. Классы `Controller` открывают доступ к информации контекста через свойство `ActionContext`,
а также набор удобных свойств (см. главу 31).

* `ViewContext` - устанавливает свойство `ViewContext`, чтобы предоставить данные контекста для
операций с представлениями, включая вспомогательные функции дескриптора (см. главу 23).

* `ViewComponentContext` - устанавливает свойство `ViewComponentContext` для компонентов
предствлений (см. главу 22).

* `ViewDataDictionary` - устанавливает свойство `ViewDataDictionary`, чтобы предоставить доступ
к данным привязки модели (см. главу 26).


Пример использования `ControllerContext` (из главы 17 в `PocoController`):
```cs
public class PocoController
{
    [ControllerContext]
    public ControllerContext ControllerContext { get; set; }

    public ViewResult Index()
    {
        var viewResult = new ViewResult
        {
            ViewName = "Result",
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = "This is a POCO Cotroller"
            }
        };

        return viewResult;
    }

    public ViewResult Headers()
    {
        var viewResult = new ViewResult
        {
            ViewName = "DictionaryResult",
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = ControllerContext.HttpContext.Request.Headers
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First())
            }
        };

        return viewResult;
    }
}
```