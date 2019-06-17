# Получение данных контекста

3 способа:
* Извлечение данных из набора объектов контекста
* Получение данных как параметра метода действия
* Явное обращение к средству привязки моделей инфраструктуры


## Получение данных из объектов контекста

Свойства для данных контекста класса `Controller`:
* `Request` - объект `HttpRequest`. Описывает запрос от клиента.
* `Response` - объект `HttpResponse`. Создание ответа клиенту.
* `HttpContext` - объект `HttpContext`. Содержит множество данных запроса.
* `RouteData` - объект `RouteData`, выпускаемый системой маршрутизации при совпадении с запросом.
(Глаыв 15 и 16).
* `ModelState` - объект `ModelStateDictionary`. Проверка достоверности данных, отправленных клиентом.
(Глава 27).
* `User` - объект `ClaimsPrincipal`. Описывает пользователя, сделавшего запрос. (Главы 29 и 30).

Многие контроллеры пишутся без применения этих свойств, потому что данные контекста доступны
через средства, рассматриваемые в следующих главах (например, `Request` доступно через привязки
моделей - глава 26).
Тем не менее, пример доступа к заголовкам в HTTP-запросе
(см. `Controllers/DerivedController.Headers()`):
```cs
public ViewResult Headers()
{
    var result = Request.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First());
    return View("DictionaryResult", result);
}
```

Наиболее часто используемые свойства `HttpRequest`:
* `Path` - раздел пути URL запроса
* `QueryString` - раздел строки запроса URL запроса
* `Headers` - словарь заголовков запроса, индексированный по именам
* `Body` - поток, который может применяться для чтения тела запроса
* `Form` - словарь данных формы в запросе, индексированный по именам
* `Cookies` - словарь cookie-наборов запроса, индексированный по именам


## Получение данных контекста в контроллере POCO

Чтобы получить данные контекста, контроллер POCO должен запросить из представление у MVC.
(см. `Controllers/PocoController`)

Для получения данных контекста определяется свойство `ControllerContext` типа `ControllerContext`,
которое декорировано атрибутом `ControllerContext`:
```cs
[ControllerContext]
public ControllerContext ControllerContext { get; set; }
```

Самые важные свойства `ControllerContext`:
* `ActionDescriptor` - объект `ActionDescriptor`, описывает метод действия
* `HttpContext` - объект `HttpContext`, детали HTTP-запроса и HTTP-ответа
* `ModelState` - объект `ModelStateDictionary`, проверка достоверности данных отправляемых клиенту\
(глава 27)
* `RouteData` - объект `RouteData`, описывает способ, которым система маршрутизации обработала
запрос

Самые важные свойства `HttpContext`:
* `Connection` - объект `ConnectionInfo`, низкоуровневое подключение к клиенту
* `Request` - объект `HttpRequest`, HTTP-запрос, полученный от клиента (см. ранее в этой главе)
* `Response` - объект `HttpResponse`, создание ответа, который будет возвращен клиенту (см. далее в этой главе)
* `Session` - объект `ISession`, описывает сеанс, с которым ассоциирован запрос
* `User` - объект `ClaimsPrincipal`, описывает пользователя, ассоциированного с запросом (глава 28)

Атрибут `ControllerContext` сообщает инфраструктуре MVC о необходимости установки значения свойства
`ControllerContext`. Это обеспечивает механизм *внедрения зависимостей* (см. главу 18).

Пример использования `ControllerContext` (из `Controllers/PocoController`):
```cs
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
```


## Использование параметров метода действия

Определенные данные контекста можно получить через параметры метода действия.
Пример, получение данных формы, отправленной пользователем (см. `Controllers/InputController`)


### Получение данных формы через объекты контекста

Пример, из `Controllers/InputController`:
```cs
public ViewResult IndexFirst() => View("SimpleFormFirst");

public ViewResult ReceiveFormFirst()
{
    var name = Request.Form["name"];
    var city = Request.Form["city"];
    return View("Result", $"{name} lives in {city}");
}
```

Метод действия `InputController.IndexFirst()` визуализирует `Views/Input/SimpleFormFirst`.
В этом представлении есть два элемента `input` (`name` и `city`):
```html
<div>
    <label for="name">Name:</label>
    <input name="name"/>
</div>
<div>
    <label for="city">City:</label>
    <input name="city"/>
</div>
```

Метод `ReceiveFormFirst()` через `Request.Form` получает данные из формы.


### Получение данных формы через параметры метода действия (рекомендуется)

Пример, из `Controllers/InputController`:
```cs
public ViewResult IndexSecond() => View("SimpleFormSecond");

public ViewResult ReceiveFormSecond(string name, string city) =>
    View("Result", $"{name} lives in {city}");
```

Аналогично предыдущему случаю.
Метод действия `InputController.IndexSecond()` визуализирует `Views/Input/SimpleFormSecond`.
В этом представлении есть два элемента `input` (`name` и `city`):
```html
<div>
    <label for="name">Name:</label>
    <input name="name"/>
</div>
<div>
    <label for="city">City:</label>
    <input name="city"/>
</div>
```

Но здесь MVC автоматически предоставляет значения для параметров из объектов контекста, в том числе:
* `Request.Form`
* `Request.QueryString`
* `RouteData.Values`

Имена параметров трактуются как нечувствительные к регистру.
