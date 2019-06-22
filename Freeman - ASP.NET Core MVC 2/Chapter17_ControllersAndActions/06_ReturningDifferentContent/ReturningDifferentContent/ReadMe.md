# Возвращение разных типов содержимого

HMTL - не единственный вид ответа от методов действий, есть и еще.

Результаты действий для содержимого:
* `JsonResult` - Метод класса Controller `Json`.
Описание: Сериализирует объект в формат JSON и возвращает его клиенту.

* `ContentResult` - Метод класса Controller `Content`.
Описание: Отправляет ответ, тело, которого содержит указанный объект.

* `ObjectResult` - Метод класса Controller `-`.
Описание: Применяет согласование содержимого для отправки объекта клиенту.

* `OkObjectResult` - Метод класса Controller `Ok`.
Описание: Использует согласование содержимого для отправки объекта клиенту с кодом состояния
HTTP 200, если согласование содержимого оказалось успешным.

* `NotFoundObjectResult` - Метод класса Controller `NotFound`.
Описание: Применяет согласование содержимого для отправки объекта клиенту с кодом состояния
HTTP 404, если согласование содержимого оказалось успешным.


## Генерирование ответа JSON

Пример ответа JSON (см. `Controllers/JsonController`):
```cs
public JsonResult Index() =>
    Json(new[] { "Alice", "Bob", "Joe" });
```

В результате броузер покажет это:
```json
["Alice","Bob","Joe"]
```


## Модульное тестирование. Результаты действий, отличающиеся от HTML

Пример (из `JsonTests)`:
```cs
[Fact]
public void JsonActionMethod()
{
    // Arrange
    var controller = new JsonController();

    // Act
    var result = controller.Index();

    // Assert
    Assert.Equal(new[] { "Alice", "Bob", "Joe" }, result.Value);
}
```

В классе `JsonResult` свойство `Value`, возвращающее данные, которые могут быть преобразованы
в формат JSON для формирования ответа клиенту.


## Использование объектов для генерации ответов

Иногда требуется в ответе возвращать содержимое специфического типа.
Простейшим является класс `ContentResult`, создаваемый посредством `Content()`, который применяется
для отправки значения `string` с дополнительным типом содержания MIME.

Пример (из `Controllers/ContentController`):
```cs
public ContentResult Index() =>
    Content("[\"Alice\", \"Bob\", \"Joe\"]", "application/json");
```

Опасность такого подхода в том, что клиент не всегда может обработать ответ в определенном формате.

Более надежный подход - согласовывать содержимое, которое выполняется классом `ObjectResult`.
Пример (из `Controllers/ObjectResultController`):
```cs
public ObjectResult Index() =>
    Ok(new string[] { "Alice", "Bob", "Joe" });
```

Процесс *согласования содержимого*:
1. Браузер делает HTTP-запрос, он включает в него заголовок `Accept`, который указывает какие
форматы брузер сможет обработать. Поддерживаемые форматы выражаются как типа MIME.
Процесс согласования содержимого более подробно описан в главе 20.


## Реагирование с помощью содержимого файлов

Большинство приложений при доставке содержимого файлов рассчитывают на промежуточное ПО статических
файлов, но есть набор результатов действий, которые можно применять для отправки файлов клиенту.

Результаты действий для файлов:
* `FileContentResult` - Метод класса Controller `File()`.
Описание: Посылает клиенту байтовый массив с указанным типом MIME.

* `FileStreamResult` - Метод класса Controller `File()`.
Описание: Читает поток и отправляет содержимое клиенту.

* `VirtualFileResult` - Метод класса Controller `File()`.
Описание: Читает поток из виртуального пути (относительно к каталогу, где размещается приложение).

* `PhysicalFileResult` - Метод класса Controller `PhysicalFile()`.
Описание: Читает содержимое файла из указанного пути и посылает его клиенту.

Пример возвраения файла в качестве результата метода действия
(см. `Controllers/VirtualFileResultController`):
```cs
public VirtualFileResult Index() =>
    File("/Files/Text.txt", "text/plain");
```

Мое примечание. Чтобы можно было получить содержимое файла нужно:
1. Добавить в конвейер `Startup.Configure` строку: `app.UseStaticFiles();`.s
2. В директорию `wwwroot` положить нужный файл. Здесь я положил файл так: `wwwroot/Files/Text.txt`.


## Возвращение ошибок и кодов HTTP

Можно отправлять клиенту специфичные сообщения об ошибках и результиркющие коды HTTP.
Большинство приложений не нуждается в таких средствах, т.к. инфраструктура MVC генерирует эти виды
результатов автоматически.

* `StatusCodeResult` - Метод класса Controller `StatusCode()`.
Описание: Отправляет клиенту указанный код состояния HTTP.

* `OkResult` - Метод класса Controller `Ok()`.
Описание: Отправляет клиенту код состояния HTTP 200.

* `CreatedResult` - Метод класса Controller `Created()`.
Описание: Отправляет клиенту код состояния HTTP 201.

* `CreatedAtActionResult` - Метод класса Controller `CreatedAtAction()`.
Описание: Отправляет клиенту код состояния HTTP 201 вместе с URL в заголовке Location,
который нацелен на действие и контроллер.

* `CreatedAtRouteResult` - Метод класса Controller `CreatedAtRoute()`.
Описание: Отправляет клиенту код состояния HTTP 201 вместе с URL в заголовке Location,
который сгенерирован из специфического маршрута.

* `BadRequestResult` - Метод класса Controller `BadRequest()`.
Описание: Отправляет клиенту код состояния HTTP 400.

* `UnauthorizedResult` - Метод класса Controller `Unauthorized()`.
Описание: Отправляет клиенту код состояния HTTP 401.

* `NotFoundResult` - Метод класса Controller `NotFound()`.
Описание: Отправляет клиенту код состояния HTTP 404.

* `UnsupportedMediaTypeResult` - Метод класса Controller `-`.
Описание: Отправляет клиенту код состояния HTTP 415.


## Отправка специфического результирующего кода HTTP

Пример (из `Controllers/StatusCodeController`):
```cs
public StatusCodeResult Index() =>
    StatusCode(StatusCodes.Status404NotFound);
```

Метод `StatusCode()` принимает значение `int`, в котором можно напрямую указывать код состояния.


## Отправка результата 404 с использованием удобного класса

Можно послать код 404 более удобным способом (из `Controllers/StatusCodeController`):
```cs
public StatusCodeResult MuchBetter() =>
    NotFound();
```


## Модульное тестирование. Коды состояния HTTP

Пример (см. `StatusCodeTests`):
```cs
[Fact]
public void NotFoundActionMethod()
{
    // Arrange
    var controller = new StatusCodeController();

    // Act
    var result = controller.MuchBetter();

    // Assert
    Assert.Equal(404, result.StatusCode);
}
```

Свойство `StatusCode` возвращает числовой код состояния HTTP,
свойство `StatusDescription` - связанную описательную строку.


## Другие классы результатов действий

* `PartialViewResult` - Метод класса Controller `PartialView()`.
Описание: Применяется для выбора частичного представления (глава 21).

* `ViewComponentResult` - Метод класса Controller `ViewComponent()`.
Описание: Используется для выбора компонента представления (глава 22).

* `EmptyResult` - Метод класса Controller `-`.
Описание: Ничего не делает и производит пустой ответ для клиента.

* `ChallengeResult` - Метод класса Controller `-`.
Описание: Применяется для обеспечения соблюдения политик безопасности в запросах (глава 30).
