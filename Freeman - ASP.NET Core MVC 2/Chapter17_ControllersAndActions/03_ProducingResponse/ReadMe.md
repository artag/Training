# Генерирование ответа

## Генерирование ответа с использованием объекта контекста

Самый низкоуровневый способ. Использует `HttpResponse`.

Базовые свойства `HttpResponse`:
* `StatusCode` - установка кода состояния HTTP, связанного с ответом
* `ContentType` - установка заголовка Content-Type ответа
* `Headers` - словарь заголовков HTTP, которые будут включены в ответ
* `Cookies` - коллекция для добавления cookie-наборов к ответу
* `Body` - объект `IO.Stream`, для записи данных тела запроса

Пример прямого способа возвращения ответа. Не рекомендуется использовать.
Из `Controllers/ResponseController`:
```cs
public void SendNotRecommendedResponse(string name, string city)
{
    Response.StatusCode = 200;
    Response.ContentType = "text/html";

    var content = Encoding.ASCII.GetBytes(
        $"<html><body>{name} lives in {city}</body></html>");

    Response.Body.WriteAsync(content, 0, content.Length);
}
```

## Понятие результатов действий. Пример создания своего класса

Более удобное средство для построения ответа.
Объект реализации `IActionResult` - *результат действия*, описывает каким должен быть ответ из
контроллера (визуализация представления, перенаправление на другой URL).

```cs
public interface IActionResult
{
    Task ExecuteResultAsync(ActionContext context);
}
```

Когда метод возвращает ответ, инфраструктура MVC вызывает `ExecuteResultAsync()`.
`ActionContext` - данные контекста для генерации ответа, включая `HttpResponse`.

Пример создания реализации `IActionResult` (см. в `Infrastructure/CustomHtmlResult`):
```cs
public class CustomHtmlResult : IActionResult
{
    public string Content { get; set; }

    public Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = 200;
        context.HttpContext.Response.ContentType = "text/html";
        var content = Encoding.ASCII.GetBytes(Content);

        return context.HttpContext.Response.Body.WriteAsync(content, 0, content.Length);
    }
}
```

Использование этой реализации (см. в `Controllers/ResponseController`):
```cs
public IActionResult SendCustomHtmlResult(string name, string city)
{
    return new CustomHtmlResult
    {
        Content = $"{name} lives in {city}"
    };
}
```


## Генерирование HTML-ответа. Класс ViewResult

Класс `ViewResult` - результат действия, предоставляющий доступ к механизму визуализации Razor.
Razor обрабатывает файлы `.cshtml` и отправляет результат клиенту через `HttpResponse`.

Пример использования класса (из `Controllers/ResponseController`):
```cs
public ViewResult SendViewResult(string name, string city) =>
    View("Result", $"{name} lives in {city}");
```

Класс `Controller` позволяет выбирать из нескольких версий `View()`:
* `View()` - стандартное представление, ассоциированное с методом действия.
(метод `MyAction()` создаст `MyAction.cshtml`).
Данные модели не используются.

* `View(view)` - указанное представление.
(`View("MyView)"` создаст `MyAction.cshtml`).
Данные модели не используются.

* `View(model)` -  стандартное представление, ассоциированное с методом действия.
Применяет указанный объект как данные модели.

* `View(view, model)` - указанное представление с передачей данных модели.


### Поиск файла представления

По умолчанию ищется тут:
`/Views/<Имя контроллера>/<Имя представления>.cshtml`
`/Views/Shared/<Имя представления>.cshtml`

Если контроллер принадлежит области (см. главу 16):
`/Areas/<Имя области>/Views/<Имя контроллера>/<Имя представления>.cshtml`
`/Areas/<Имя области>/Views/Shared/<Имя представления>.cshtml`
`/Views/Shared/<Имя представления>.cshtml`


### Модульное тестирование. Визуализация представления.

Пример см. в `ActionTests`:
```cs
[Fact]
public void ViewSelected()
{
    // Arrange
    var controller = new ResponseController();

    // Act
    var result = controller.SendViewResult("Adam", "London");

    // Assert
    Assert.Equal("Result", result.ViewName);
}
```

При тестировании метода действия, выбирающего стандартное представление
(см. `Controllers/HomeController`):
```cs
...
public IActionResult Index() => View();
...
```

тест будет выглядеть вот как-то так (см. `ActionTests`):
```cs
[Fact]
public void DefaultView()
{
    // Arrange
    var controller = new HomeController();

    // Act
    var result = controller.Index();

    // Assert
    Assert.Null(result.ViewName);
}
```


## Передача данных из метода действия в представление

### Использование объекта модели представления

#### Нетипизированное (слаботипизированное) представление

Пример передачи `DateTime` из Action во View (см. `Controllers/ModelController`):
```cs
public ViewResult SendToUntypedView() => View(DateTime.Now);
```

Содержимое View (см. `Views/Model/SendToUntypedView`):
```html
@{ Layout = "_Layout"; }

<b>Model</b>: @(((DateTime)Model).DayOfWeek)
```

Такой View является *нетипизированным* или *слаботипизированным*: тип модели трактуется как `object`.
Необходимо приведение типа.


#### Строго типизированное представление

Пример передачи `DateTime` из Action во View (см. `Controllers/ModelController`):
```cs
public ViewResult SendToTypedView() => View(DateTime.Now);
```

Содержимое View (см. `Views/Model/SendToTypedView`):
```html
@model DateTime

@{ Layout = "_Layout"; }

<b>Model</b>: @Model.DayOfWeek
```

Такой View является *строго типизированным*, т.к. содержит детали типа объекта модели представления
(строка: `@model DateTime`).


#### Возможное затруднение при передаче string в модели представления

Нельзя просто так взять и передать string во View:
```cs
public ViewResult Result() => View("Hello, World");
```

Здесь MVC будет искать представление по имени `Hello, World.cshtml`.

Надо делать так (см. `Controllers/ModelController`):
```cs
public ViewResult SendStringToView() => View((object)"Hello, World");
```

Такой трюк с моделью представления (`(object)"Hello, World"`) обеспечивает грамотную передачу строки
во View.

Содержимое View (см. `Views/Model/SendStringToView`):
```html
@model string

@{ Layout = "_Layout"; }

<b>Model</b>: @Model
```


### Модульное тестирование. Объекты модели представления.

Проверка типа модели. Пример см. в `SendingModelTests`:
```cs
[Fact]
public void ModelObjectType()
{
    // Arrange
    var controller = new ModelController();

    // Act
    var result = controller.SendToTypedView();

    // Assert
    Assert.IsType<System.DateTime>(result.ViewData.Model);
}
```


### Передача данных с помощью ViewBag

`ViewBag` это динамический объект, который позволяет определять в себе свойства и получать
к ним доступ в представлении. `ViewBag` предоставляется классом `Controller`.

Использование `ViewBag` в контроллере (см. `Controllers/ViewBagController`):
```cs
public ViewResult SendViewBagToView()
{
    ViewBag.Message = "Hello";
    ViewBag.Date = DateTime.Now;

    return View();
}
```

И чтение данных из `ViewBag` во View (см. `Views/ViewBag/SendViewBagToView):
```html
@{ Layout = "_Layout"; }

<p>
    The day is: @ViewBag.Date.DayOfWeek
</p>
<p>
    The message is: @ViewBag.Message
</p>
```


### Модульное тестирование. Объект ViewBag.

Пример см. в `SendingViewBagTests`:
```cs
[Fact]
public void ModelObjectType()
{
    // Arrange
    var controller = new ViewBagController();

    // Act
    var result = controller.SendViewBagToView();

    // Assert
    Assert.IsType<string>(result.ViewData["Message"]);
    Assert.Equal("Hello", result.ViewData["Message"]);
    Assert.IsType<System.DateTime>(result.ViewData["Date"]);
}
```
