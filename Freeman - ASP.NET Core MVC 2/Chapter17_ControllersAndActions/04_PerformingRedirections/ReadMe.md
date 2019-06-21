# Выполнение перенаправления

Когда выполняется перенаправление, браузеру отправляется один из двух HTTP кодов:
1. Код HTTP 302 - *временное перенаправление*. Наиболее часто используемый тип перенаправления.
Именно его необходимо посылать в случае применения паттерна `Post/Redirect/Get`.

2. Код HTTP 301 - *постоянное перенаправление*. Должен использоваться с осторожностью, т.к. он
инструктирует получателя не запрашивать снова исходный URL, а применять новый URL, включенный
вместе с кодом перенаправления.

Для выполнения перенаправления могут применяться и другие результаты действий:
* `RedirectResult`
  * Метод класса Controller - `Redirect()` и `RedirectPermanent()`.
  * Описание: Посылает ответ с кодом состояния HTTP 301 или 302, выполняя перенаправление клиента.
    на новый URL

* `LocalRedirectResult`
  * Метод класса Controller: `LocalRedirect()` и `LocalRedirectPermanent()`.
  * Описание: Выполняет перенаправление клиента на локальный URL.

* `RedirectToActionResult`
  * Метод класса Controller - `RedirectToAction()` и `RedirectionToActionPermanent()`
  * Описание: Выполняет перенаправление клиента на указанное действие или контроллер.

* `RedirectToRouteResult`
  * Метод класса Controller - `RedirectToRoute()` и `RedirectToRoutePermanent()`
  * Описание: Выполняет перенаправление клиента на URL, сгенерированный из специфического маршрута.


## Перенаправление на буквальный URL

Наиболее базовый способ перенаправления браузера - вызов `Redirect()`
(см. пример в `Controllers/RedirectionController`):
```cs
...
public RedirectResult ActionRedirect() =>
    Redirect("/Redirection/LiteralUrl");

public ViewResult LiteralUrl() =>
    View("Result", $"Result from {nameof(LiteralUrl)}");
...
```

`"/Redirection/LiteralUrl"` - URL временного перенаправления.

Постоянное перенаправление будет выглядеть так:
```cs
public RedirectResult ActionRedirect() => RedirectPermanent("/Redirection/LiteralUrl");
```


## Модульное тестирование. Перенаправление на буквальный URL

Пример теста (из `RedirectTests`):
```cs
[Fact]
public void Redirection()
{
    // Arrange
    var controller = new RedirectionController();

    // Act
    var result = controller.ActionRedirect();

    // Assert
    Assert.Equal("/Redirection/LiteralUrl", result.Url);
    Assert.False(result.Permanent);
}
```


## Перенаправление на URL системы маршрутизации

Задействование системы маршрутизации и генерация URL выполняется при помощи `RedirectToRoute()`.
Пример (см. `Controllers/RedirectionController`):
```cs
...
public RedirectToRouteResult ActionRedirectToRoute()
{
    var route = new
    {
        controller = "Redirection", action = "RoutedRedirection", id = "MyID"
    };

    return RedirectToRoute(route);
}

public ViewResult RoutedRedirection() =>
    View("Result", $"Result from {nameof(RoutedRedirection)}");
...
```

`RedirectToRoute()` выпускает временное перенаправление.
Для постоянного перенаправления использовать `RedirectToRoutePermanent`.

Оба свойства принимают анонимный тип, свойства которого передаются системе маршрутизации для
генерирования URL.


## Модульное тестирование. Перенаправление на URL системы маршрутизации

```cs
[Fact]
public void Redirection()
{
    // Arrange
    var controller = new RedirectionController();

    // Act
    var result = controller.ActionRedirectToRoute();

    // Assert
    Assert.False(result.Permanent);
    Assert.Equal("Redirection", result.RouteValues["controller"]);
    Assert.Equal("RoutedRedirection", result.RouteValues["action"]);
    Assert.Equal("MyID", result.RouteValues["id"]);
}
```


## Перенаправление на метод действия

Перенаправление на метод действия - `RedirectToAction` (или `RedirectToActionPermanent`).
Пример (см. `Controllers/RedirectionController`):
```cs
...
public RedirectToActionResult ActionRedirectToAction() =>
    RedirectToAction(nameof(ActionRedirection));

public ViewResult ActionRedirection() =>
    View("Result", $"Result from {nameof(ActionRedirection)}");
...
```

Если указан только метод действия, то предполагается, что он относится к текущему контроллеру.
Если надо перенаправить на метод другого контроллера, то:
(см. `Controllers/RedirectionController` и `Controllers/OtherController`):
```cs
...
public RedirectToActionResult OtherActionRedirectToAction()
{
    var action = nameof(OtherController.ActionRedirection);
    var controller = nameof(OtherController).Replace("Controller", "");

    return RedirectToAction(action, controller);
}
...
```

```cs
public ViewResult ActionRedirection() =>
    View("Result", $"Result from {nameof(OtherController)}.{nameof(ActionRedirection)}");
```


## Модульное тестирование. Перенаправление на метод действия

```cs
[Fact]
public void Redirection()
{
    // Arrange
    var controller = new RedirectionController();

    // Act
    var result = controller.ActionRedirectToAction();

    // Assert
    Assert.False(result.Permanent);
    Assert.Equal("ActionRedirection", result.ActionName);
}
```
