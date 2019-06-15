# Настройка системы маршрутизации

## Изменение конфигурации системы маршрутизации

В главе 15 было показано использование объекта `RouteOptions` в `Startup.ConfigureServices()`
для настройки специального ограничения маршрута:
```cs
...
services.Configure<RouteOptions>(
             options => options.ConstraintMap.Add("weekday", typeof(WeekDayConstraint)));
...
```

Здесь конфигурируется объект `RouteOptions`, который управляет рядом линий поведения системы
маршрутизации.
В словарь `ConstraintMap` добавляется новое отображение, поэтому на новое специальное ограничение
`WeekDayConstraint` можно ссылаться встраиваемым образом через `weekday`.

В данной главе рассматриваются дополнительное применение `RouteOptions`:
* `AppendTrailingSlash` - когда это свойство == `true`, к генерируемым URL добавляется `/`.
Стандартное значение `false`.
* `LowercaseUrls` - когда это свойство == `true`, результирующие URL преобразуются в нижний регистр,
если контроллер, действие или значения сегментов содержат символы нижнего регистра.
Стандартное значение `false`.

Пример изменения этих свойств `RouteOptions (из `Startup.ConfigureServices()`):
```cs
...
services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});
...
```

Можно увидеть в выводе на экран (через `Views/Shared/Result.cshtml`), что при вводе URL типа
`/Home/List/Hello` генерируется URL вида: `/home/list/hello/`


## Создание специального класса маршрута

Интерфейс `Microsoft.AspNetCore.Routing.IRouter` используется для создания специального маршрута.
Его определение:
```cs
public interface IRouter
{
    Task RouteAsync(RouteContext context);
    VirtualPathData GetVirtualPath(VirtualPathContext context);
}
```

`RouteAsync()` используется для обработки входящих запросов
`GetVirtualPath()` используется для генерации исходящих URL

### Пример. Создание специального маршрута для запросов к старым URL

Класс `Infrastructure/LegacyRoute` реализует `IRouter`.

Здесь определяется только `RouteAsync()`, который обрабатывает входящие URL.
`RouteAsync()` оценивает может ли запрос быть обработан и, если может, за управление процессом,
который генерирует ответ, отправляемый обратно клиенту.
Процесс выполняется асинхронно, поэтому `RouteAsync()` возвращает объект `Task`.

Немного кода:
```cs
public Task RouteAsync(RouteContext context)
{
    // Получение полного пути URL, с обрезанием косой черты.
    var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');

    // Если запрошенный URL входит в число сконфигурированных URL (_urls)
    if (_urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
    {
        // Установка свойства Handler с применением лямбда-функции, генерирующей ответ.
        context.Handler = async ctx =>
        {
            HttpResponse response = ctx.Response;
            byte[] bytes = Encoding.ASCII.GetBytes($"URL: {requestedUrl}");
            await response.Body.WriteAsync(bytes, 0, bytes.Length);
        };
    }

    return Task.CompletedTask;
}
```

`RouteContext` - предоставляет доступ ко всем данным запроса и определяет 3 свойства:
* `RouteData` - используется для определения контроллера, метода действия и аргументов, применяемых
для обработки запроса.
* `HttpContext` - предоставляет доступ к деталям HTTP-запроса и предназначен для выпуска HTTP-ответа.
* `Handler` - свойство `RequestDelegate`. Обрабатывает запрос.
Если `RouteAsync()` не устанавливает это свойство, то система маршрутизации продолжит работу своим
способом посредством набора маршрутов в конфигурации приложения.

Система маршрутизации вызывает `RouteAsync()` каждого маршрута в приложении.
Если `Handler` установлено в `RequestDelegate`, тогда маршрут предоставляет системе маршрутизации
делегат и этот делегат вызывается для генерации ответа.

Сигнатура делегата `RequestDelegate`:
```cs
public delegate Task RequestDelegate(HttpContext context);
```

Если свойство `Handler` не установлено ни в одном из маршрутов, то система маршрутизации
генерирует ответ `404 - Not Found`.

В рассматриваемом примере применяется `HttpContext.Request` (`HttpRequest`), предоставляющий доступ
ко всей информации о запросе.

`HttpContext.Response` (`HttpResponse`) используется для создания ответу клиенту.

`HttpResponse.Body.WriteAsync` используется для асинхронной записи в качестве ответа простой строки
ASCII.
Такой прием обычно не используется в "боевых" условиях. Обычно используется вспомогательный
контроллер с генерацией нужного представления (см. следующий раздел).


### Применение специального класса маршрута

`MapRoute()` не поддерживает специальные классы маршрутов.
Используется другой поход (см. Startup.Configure()):
```cs
routes.Routes.Add(new LegacyRoute(
                      "/articles/Windows_3.1_Overview.html",
                      "/old/.NET_1.0_Class_Library"));
```

Теперь, запросив, например, URL `/articles/Windows_3.1_Overview.html`, можно увидеть результат
действия специального класса `LegacyRoute`.


### Специальная маршрутизация на контроллеры MVC

Чаще всего спеыиальный класс маршрута используется вместе с соответствующим контроллером
(см. класс `Controllers/LegacyController.cs` + `Views/Legacy/GetLegacyUrl.cshtml`).

Класс `Infrastructure/LegacyRouteForController` маршрутизирует обрабатываемые URL на действие
`GetLegacyUrl` контроллера `Legacy`.

При инициализации `LegacyRouteForController` используется `MvcRouteHandler`.
`MvcRouteHandler` применяется для нахождения контроллеров, действий и возвращения результата клиенту.

В LegacyRouteForController.RouteAsync():
```cs
public async Task RouteAsync(RouteContext context)
{
    // Получение полного пути URL, с обрезанием косой черты.
    var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');

    // Если запрошенный URL входит в число сконфигурированных URL (_urls)
    if (_urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
    {
        context.RouteData.Values["controller"] = "Legacy";
        context.RouteData.Values["action"] = "GetLegacyUrl";
        context.RouteData.Values["legacyUrl"] = requestedUrl;
        await _mvcRoute.RouteAsync(context);
    }
}
```

`context.RouteData.Values` - словарь, который предоставляет данные объекту `MvcRouteHandler`.
Поиск и использование класса контроллера для обработки запроса возлагается на `MvcRouteHandler`:
```cs
...
await _mvcRoute.RouteAsync(context);
...
```

`RouteContext`, который содержит значения `controller`, `action` и `legacyUrl` передается
`_mvcRoute.RouteAsync()`, который сам обрабатывает запрос и устанавливает свойство `Handler`.


### Применение специальной маршрутизации на контроллеры MVC

См. файл `Startup.Configure()`:
```cs
routes.Routes.Add(new LegacyRouteForController(
                      app.ApplicationServices,
                      "/oldLink1",
                      "/oldLink2"));
```


### Генерирование исходящих URL

Для поддержки генерации исходящих URL надо реализовать в классе специальной маршрутизации метод
`GetVirtualPath()`.
Из класса `Infrastructure/LegacyRouteForController.GetVirtualPath()`:
```cs
public VirtualPathData GetVirtualPath(VirtualPathContext context)
{
    if (context.Values.ContainsKey("legacyUrl"))
    {
        var url = context.Values["legacyUrl"] as string;
        if (_urls.Contains(url))
        {
            return new VirtualPathData(this, url);
        }
    }

    return null;
}
```

Система маршрутизации вызывает метод `GetVirtualPath()` для каждого маршрута, давая каждому шанс
сгенерировать исходящий URL, требующийся приложению.

`VirtualPathContext` содержит свойства:
* `RouteName` - имя маршрута.
* `Values` - словарь значений для переменных сегментов.
* `AmbientValues` - словарь значений, полезных при генерации URL, но которые не встраиваются в
результат. При реализации собственного класса маршрутизации такой словарь обычно пуст.
* `HttpContext` - объект `HttpContext`, содержит информацию о запросе и ответе.

В примере используется свойство `Values`.


### Посмотреть сгенерированные исходящие URL

См. файл `Views/Shared/Result.cshtml`:
```html
<div>
    <a asp-route-legacyurl="/oldLink1">
        This is outgoing URL from LegacyRouteForController
    </a>
</div>

<div>
    <p>
        URL: @Url.Action(null, null, new { legacyurl = "/oldLink1" })
    </p>
</div>
```

Здесь генерируется ссылка и простая текстовая строка на этот URL `/oldLink1`.
