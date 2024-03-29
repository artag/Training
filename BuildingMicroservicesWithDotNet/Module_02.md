# Module 2. Your first microservice

## Lesson 8. Creating a microservice via the .NET CLI

### Новый Web API проект

```text
dotnet new webapi -n Play.Catalog.Service
```

### Рассказано про:

* `Program.cs`
* `Startup.cs`
* `WeatherForecast.cs`
* Директория `Controllers` и `Controllers/WeatherForecastController.cs`
* `.vscode/launch.json` определяет how VS Code launches the app
* `.vscode/tasks.json` определяет how VS Code starts a build

В `Properties/launchSettings.json` microservice addresses "applicationUrl":

```text
"applicationUrl": "https://localhost:5001;http://localhost:5000",
```

### Build проект

Из директории, где лежит файл `*.csproj`:

```text
dotnet build
```

Ну или задать файл `.vscode/tasks.json` и нажать `Ctrl+Shift+B` (я поставил `F6`).

### Run проект

Из директории, где лежит файл `*.csproj`:

```text
dotnet run
```

Ну или задать файл `.vscode/launch.json` и нажать `Ctrl+Shift+D` - запуск, `F5` - запуск в режиме
отладки.

### Задать сертификат для HTTPS development

Из терминала, из директории, где лежит файл `*.csproj`:

```text
dotnet dev-certs https --trust
```

Для Linux появляется следующее сообщение:

"Trusting the certificate on Linux distributions automatically is not supported. For instructions
on how to manually trust the certificate on your Linux distribution, go to
https://aka.ms/dev-certs-trust"

### Entry address for web api project

`https://localhost:5001/swagger/index.html`

## Lesson 9. Introduction to the REST API and DTOs

The REST API defines the operations exposed by the microservice.

| Operation               | Description                  |
|-------------------------|------------------------------|
| `GET /items`            | Retrieves all items          |
| `GET /items/{id}`       | Retrieves the specified item |
| `POST /items`           | Creates an item              |
| `PUT /items/{id}`       | Updates the specified item   |
| `DELETE /items/{id}`    | Deletes the specified item   |

A *Data Transfer Object* (*DTO*) is an object that carries data between processes.

The DTO represents the **contract** between the microservice API and the client.

## Lesson 10. Adding the DTOs

В качестве DTO удобно использовать *Record Type*:

* Simpler to declare
* Value-based equality
* Immutable by default
* Built-in `ToString()` override

Пример record:

```csharp
public record CreateItemDto(string Name, string Description, decimal Price);
```

## Lesson 11. Adding the REST API operations

The *controller* groups the set of actions that can handle API requests.

### Для контроллера

Базовый класс `ControllerBase` определяет свойства и методы, используемые для HttpRequests.

`[ApiController]` включает некоторые полезные фичи, такие как: Model Validation Errors или
bindings для параметров в методах.

`[Route("items")]` определяет url pattern, к которому будет mapped контроллер.
Для `items` контроллер будет доступен по адресу `https://localhost:5001/items`.

### Методы

`[HttpGet]` - атрибут для метода, выполняющего роль GET в REST API.

`[HttpGet("{id}")]` - Часть из route адреса будет передаваться в качестве параметра `id` в метод.
Например из адреса `/items/12345` в качестве `id` будет передано `12345`.

`ActionResult` позволяет вернуть определенный http status code, типа: 200 (OK), 400 (Bad request).
Также может вернуть more specific type, типа: Detail Type.

`[HttpPost]` - атрибут для метода, выполняющего роль POST в REST API.

`[HttpPut]` - атрибут для метода, выполняющего роль PUT в REST API.

`IActionResult` - определяет контракт, представляющий результат метода действия.

`[HttpPut("{id}")]` - Часть из route адреса будет передаваться в качестве параметра `id` в метод
(также как и в аттрибуте для операции GET).

`[HttpDelete]` - атрибут для метода, выполняющего роль DELETE в REST API.

### Record. Создание clone на основе старого

```csharp
var updatedItem = existingItem with
{
    Name = updateItemDto.Name,
    Description = updateItemDto.Description,
    Price = updateItemDto.Price
};
```

## Lesson 12. Handling invalid inputs

`NotFound` - возвращает код 404.

### Model validation

В DTO можно задавать ограничения на входные параметры при помощи атрибутов:

`[Required]` - параметр всегда должен быть определен (не `null`)

`[Range(0, 1000)]` - задает допустимый числовой диапазон (0 - минимальное число,
1000 - максимальное).

Пример задания:

```csharp
public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
```

При ошибке валидации возвращается код ошибки 400, в Response body, в "errors"
содержатся описания ошибок. Например:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,                          // Код ошибки
  "traceId": "00-1ce407fcaf592a47afb5afc6511f4e79-8a27425e4e987b46-00",
  "errors": {
    "Name": [
      "The Name field is required."                   // Атрибут [Required]
    ],
    "Price": [
      "The field Price must be between 0 and 1000."   // Атрибут [Range(0, 1000)]
    ]
  }
}
```
