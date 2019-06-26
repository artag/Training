# Использование фильтров авторизации

1. Применяются для реализации политики безопасности приложения.
2. Выполняются фильтрами других типов и до запуска метода действия.

Определение интерфейсов `IAuthorizationFilter` и `IAsyncAuthorizationFilter` :
```cs
public interface IAuthorizationFilter : IFilterMetadata
{
    void OnAuthorization(AuthorizationFilterContext context);
}

public interface IAsyncAuthorizationFilter : IFilterMetadata
{
    Task OnAuthorizationAsync(AuthorizationFilterContext context);
}
```

`OnAuthorization()` или `OnAuthorizationAsync` вызываются для предоставления фильтру
возможности авторизовать запрос.

Фильтр получает данные контекста через `AuthorizationFilterContext` (производный от `FilterContext`)
и добавляет одно свойство:

* `Result` - тип `IActionResult`. Свойство устанавливается фильтрами авторизации, когда запрос
не удовлетворяет политике авторизации приложения. Если оно установлено, то MVC вместо вызова
метода действия визуализирует `IActionResult`.


## Создание фильтра авторизации

Пример (из `Infrastructure/HttpsOnlyAttribute`):
```cs
public class HttpsOnlyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.IsHttps)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
```

Если запрос удовлетворяет политике авторизации, то фильтр ничего не делает.

При наличии проблемы фильтр устанавливает свойство `Result` у `AuthorizationFilterContext`, что
препятствует дальнейшему выполнению и предоставляет MVC результат для возвращения клиенту.

Здесь опять проверяется наличие https соединения.

В реальных приложениях возврат кода 403, если соединение не https, не используется. Здесь данный
прием используется для демонстрации работы фильтра авторизации.

В классе `Controllers/HomeController` применен новый фильтр `HttpsOnly`:
```cs
[HttpsOnly]
public class HomeController : Controller
{
    public IActionResult Index() =>
        View("Message", "This is the Index action on the Home controller");
}
```

Как смотреть и увидеть: запустить следующие адреса:

* `http://localhost:5000` - по этому адресу возвратится код ошибки 403
* `https://localhost:5001`


## Модульное тестирование. Фильтры

Пример (из `FilterTests`):
```cs
[Fact]
public void TestHttpsFilter()
{
    // Arrange
    var httpRequest = new Mock<HttpRequest>();
    httpRequest.SetupSequence(m => m.IsHttps)
               .Returns(true)
               .Returns(false);

    var httpContext = new Mock<HttpContext>();
    httpContext.SetupGet(m => m.Request).Returns(httpRequest.Object);

    var actionContext = new ActionContext(
        httpContext.Object, new RouteData(), new ActionDescriptor());

    var filters = Enumerable.Empty<IFilterMetadata>().ToList();
    var authContext = new AuthorizationFilterContext(actionContext, filters);

    var filter = new HttpsOnlyAttribute();

    // Act
    filter.OnAuthorization(authContext);
    Assert.Null(authContext.Result);

    // Assert
    filter.OnAuthorization(authContext);
    Assert.IsType(typeof(StatusCodeResult), authContext.Result);
    Assert.Equal(StatusCodes.Status403Forbidden, (authContext.Result as StatusCodeResult).StatusCode);
}
```

Тест начинается с создания имитированных `HttpRequest` и `HttpContext`, которые позволяют
представить запрос с или без HTTPS.

Для тестирования обоих условий:
```cs
...
httpRequest.SetupSequence(m => m.IsHttps)
               .Returns(true)
               .Returns(false);
...
```

Сконфигурированное таким образом свойство `HttpRequest` возвращает `true`, когда читается в первый
раз, и `false` - по второй раз.

`HttpContext` применяется для создания `ActionContext`, который используется для создания
`AuthorizationFilterContext`.

За счет инспектирования свойства `Result` у `AuthorizationFilterContext` производится проверка как
фильтр реагирует на запросы, отличные от HTTPS, а затем, как реагирует на запросы HTTP.
