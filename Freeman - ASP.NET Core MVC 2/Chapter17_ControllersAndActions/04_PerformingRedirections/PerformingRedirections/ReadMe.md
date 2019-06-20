# Выполнение перенаправления

Когда выполняется перенаправление, браузеру отправляется один из двух HTTP кодов:
1. Код HTTP 302 - *временное перенаправление*. Наиболее часто используемый тип перенаправления.
Именно его необходимо посылать в случае применения паттерна `Post/Redirect/Get`.

2. Код HTTP 301 - *постоянное перенаправление*. Должен использоваться с осторожностью, т.к. он
инструктирует получателя не запрашивать снова исходный URL, а применять новый URL, включенный
вместе с кодом перенаправления.

Для выполнения перенаправления могут применяться и другие результаты действий:

| Имя                      | Метод класса Controller | Описание                      | Описание                                                                                         |
| ------------------------ | ------------------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| `RedirectResult`         | `Redirect()` и `RedirectPermanent()`                    | Посылает ответ с кодом состояния HTTP 301 или 302, выполняя перенаправление клиента на новый URL |
| `LocalRedirectResult`    | `LocalRedirect()` и `LocalRedirectPermanent()`          | Выполняет перенаправление клиента на локальный URL                                               |
| `RedirectToActionResult` | `RedirectToAction()` и `RedirectionToActionPermanent()` | Выполняет перенаправление клиента на указанное действие или контроллер                           |
| `RedirectToRouteResult`  | `RedirectToRoute()` и `RedirectToRoutePermanent()`      | Выполняет перенаправление клиента на URL, сгенерированный из специфического маршрута             |


## Перенаправление на буквальный URL

Наиболее базовый способ перенапраления браузера - вызов `Redirect()`
(см. пример в `Controllers/RedirectionController`):
```cs
...
public RedirectResult Redirect() => Redirect("/Redirection/LiteralUrl");

public ViewResult LiteralUrl() => View("Result", $"Result from {nameof(LiteralUrl)}");
...
```

`"/Redirection/LiteralUrl"` - URL временного перенаправления.

Постоянное перенаправление будет выглядеть так:
```cs
public RedirectResult Redirect() => RedirectPermanent("/Redirection/LiteralUrl");
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
    var result = controller.Redirect();

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
 public RedirectToRouteResult RedirectToRoute()
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
public void Redirect()
{
    // Arrange
    var controller = new RedirectionController();

    // Act
    var result = controller.RedirectToRoute();

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
public RedirectToActionResult RedirectToAction() =>
    RedirectToAction(nameof(ActionRedirection));

public ViewResult ActionRedirection() =>
    View("Result", $"Result from {nameof(ActionRedirection)}");
...
```
