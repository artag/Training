# Building Microservices with .NET

## Lesson 5. What's wrong with the monolith?

### PROS

* Convenient for new projects
* Tools mostly focused on them
* Great code reuse
* Easier to run locally
* Easier to debug and troubleshoot
* One thing to build
* One thing to deploy
* One thing to test end to end
* One thing to scale

### CONS

* Easily gets too complex to understand
* Merging code can be challenging
* Slows down IDEs
* Long build times
* Slow and infrequent deployments
* Long testing and stabilization periods
* Rolling back is all or nothing
* No isolation between modules
* Can be hard to scale
* Hard to adopt new tech

## Lesson 6. What are microservices?

*Microservices* - an architectural style that structures an application as a collection
of independently deployable services that are modeled around a business domain and are usually
owned by a small team.

### PROS

* Small, easier to understand code base
* Quicker to build
* Independent, faster deployments and rollbacks
* Independently scalable
* Much better isolation from failures
* Designed for continuous delivery
* Easier to adopt new, varied tech
* Grants autonomy to teams and lets them work in parallel

### CONS

* Not easy to find the right set of services
* Adds the complexity of distributed systems
* Shared code moves to separate libraries
* No good tooling for distributed apps
* Releasing features across services is hard
* Hard to troubleshoot issues across services
* Can't use transactions across services
* Raises the re quired skillset for the team

### When to use microservices?

* It's perfectly fine to start woth a monolith, then move to microservices

* Start looking at microservices when:
  * The code base size is more than what a small team can mantain
  * Teams can't move fast anymore
  * Builds become too slow due to large code base
  * Time to market is compromised due to infrequent deployments and long verification times

* It's all about team autonomy!

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

## Lesson 14. Introduction to the repository pattern and MongoDB

A *repository* is an abstraction between the data layer and the business layer of an application.

Repository:

* Decouples the application logic from the data layer
* Minimizes duplicate data access logic

### MongoDB

*MongoDB* is a document-oriented NoSQL database which stores data in JSON-like documents with
dynamic schema.

NoSQL solution preferred for our microservices because:

* Won't need relationships across the data
* Don't need ACID guarantees. *ACID*: atomicity, consistency, isolation, durability
* Won't need to write complex queries
* Need low latency, high availability and high scalability

## Lesson 15. Implementing a MongoDB repository

*Enities* used by repositories to store and retrieve data.

`MongoDB.Driver` - nuget package для установки.

```text
dotnet add package MongoDB.Driver
```

Для репозитория используется asynchronous programming - enchances the overall responsiveness of
our service.

## Lesson 16. Using the repository in the controller

*Замечание*. ASP.NET Core после версии 3 убирает суффикс `Async`:

```csharp
// ItemsController.cs

[HttpPost]
public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
{
    // ...
    // GetByIdAsync превратится в GetById

    return CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id}, item);
}
```

Для выключения этого режима надо в `Startup.cs` добавить опцию
`SuppressAsyncSuffixInActionNames = false`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
    // ...
}
```

## Lesson 17. Introduction to Docker

*Docker* provides the ability to package and run an application in a loosely isolated environment
called a container.

## Lesson 18. Trying out the REST API with a MongoDB container

Для запуска контейнера:

```text
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo
```

У меня последняя версия MongoDB не запустилась (стартовала и тут же останавливалась), поэтому
я взял одну из предпоследних версий:

```text
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo:4.4.7
```

* `-d` - don't attach to the process in container.
* `--rm` - удалить контейнер когда он будет остановлен.
* `--name` - наименование контейнера.
* `-p` - port,
  * `27017:27017` - `external_port:internal_port`. External_port можно делать любым (как удобно).
* `-v` - volume. Определяется как хранятся данные контейнера.
  * `mongodbdata:/data/db`:
    * `mongodbdata` - location *outside* of the container.
    * `/data/db` - default location where MongoDB stores the database files (*inside* the container).
* `mongo` - the name of the Docker image that we want to run.

Посмотреть список запущенных контейнеров:

```text
docker ps
```

### Управление MongoDB из VS Code

To connect to MongoDB and Atlas directly from your VS Code environment, navigate your databases
and collections, inspect your schema:

В Visual Studio Code можно установить extension `MongoDB for VS Code` (by MongoDB).

Под NixOS был геморрой с подключением, под Linux Mint все завелось сразу же.

Если посмотреть внутрь БД, то можно увидеть, что запись сохранилась в следующем виде:

```json
{
  "_id": {
    "$binary": {
      "base64": "Bthc+8THu0Ws4Wx5OAb4IQ==",
      "subType": "03"
    }
  },
  "Name": "Potion",
  "Description": "Restore a small amount of HP",
  "Price": "5",
  "CreatedDate": [
    637634189696446200,
    0
  ]
}
```

#### Настройка сериализации в MongoDB (Startup.cs)

`Id` и `CreatedDate` сохранены в нечитаемом формате. Чтобы это исправить надо добавить в класс
`Startup`, в метод `ConfigureServices` следущие строки:

```csharp
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
```

Первая строка означает, что при сохранении типа `Guid` в БД он будет записан в виде `string`.
Аналогично для `DateTimeOffset` во второй строке.

#### Удаление БД

ПКМ на Catalog в Connections -> Ввести "Catalog" для подтверждения удаления БД.

#### Настроенная сериализация в MongoDB

После повторного запуска приложения и сохранения в БД новой записи, можно увидеть, что
она была сохранена в таком виде:

```json
{
  "_id": "3dd28edc-a898-4e9f-a725-46f9efd66028",
  "Name": "Potion",
  "Description": "Restore a small amount of HP",
  "Price": "5",
  "CreatedDate": "2021-08-01T13:07:54.0738536+00:00"
}
```

## Lesson 19. Introduction to Dependency Injection and Configuration

Benefits of Dependency Injection and Dependency Inversion:

* By having our code depend upon abstractions we are decoupling implementations from each other.
* Code is cleaner, easier to modify and easier to reuse.

`Service Container`, реализующий `IServiceProvider` занимается конструированием зависимостей.
Все зависимости регистрируются в `Startup.cs`.

1. Dependency register into the `Service Container`.
2. `IServiceProvider` add dependency to registered dependencies.
3. Когда требуется создать класс с какими-либо зависимостями, `Service Container`
   1. Ищет среди registered dependencies нужную dependency.
   2. Создает или использует уже созданные зависимости.
   3. Конструирует нужный класс с требуемыми зависимостями.

ASP.NET Core содержит *Configuration Sources* - хранит и предоставляет детали конфигурации
для сервисов.

Источники для Configuration Sources:

* `appsettings.json`
* Command line args
* Environment variables
* Local secrets
* Cloud

Все эти источники автоматически загружаются в configuration system когда запускается host.
Это настраивается в Host Startup (`Program.cs`).

## Lesson 20. Implementing dependency injection and configuration

Добавляется DI в `ItemsController`, `ItemsRepository`.

### Настройки

В `appsetting.json` добавляется секции `MongoDbSettings` и `ServiceSettings` для определения
настроек БД (эти настройки удаляются из `ItemsRepository`):

```json
"ServiceSettings": {
  "ServiceName": "Catalog"
},
"MongoDbSettings": {
  "Host": "localhost",
  "Port": "27017"
},
```

Для использования настроек в коде добавляются соответствующие классы в директорию `Settings`:
`MongoDbSettings.cs` и `ServiceSettings.cs`.

### Новая фича языка CSharp

```csharp
public class MongoDbSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    // ..
}
```

Вместо `set` ставится `init` - свойство больше не будет изменяться после его инициализации.

### Регистрация сервисов

Значения настроек добавляются в класс `ServiceSettings` в `Startup.cs`, метод `ConfigureServices`:

```csharp
// ..
_serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
// ..
```

Здесь происходит десериализация значения из файла конфигурации в требуемый класс,
представляющий настройку.

Для создания и регистрации `IMongoDatabase`, используется `IServiceCollection`:

```csharp
// ..
services.AddSingleton(serviceProvider =>{
    var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
    return mongoClient.GetDatabase(_serviceSettings.ServiceName);
});
// ..
```

*AddSingleton* - регистрирует тип или объект как единственный, который будет использоваться
во всем микросервисе.

Здесь довольно сложная регистрация `IMongoDatabase`, т.к. описывается процесс создания объекта.

Регистрация `IItemsRepository` проще:

```csharp
// ..
services.AddSingleton<IItemsRepository, ItemsRepository>();
// ..
```

## Lesson 22. Using Postman

Скачивается клиент Postman. Отсюда: <https://www.postman.com/downloads>

### GET запрос

1. Ставится `GET` запрос.
2. Адрес `https://localhost:5001/items`.
3. Send.

При запуске ругнется: "SSL Error: Unable to verify the first certificate". Это нормально, т.к.
сертификаты у нас самоподписанные и для разработки. Жмем на "Disable SSL Verification".

### POST запрос

1. Ставится `POST` запрос.
2. Адрес `https://localhost:5001/items`.
3. Переключение на Body.
4. Выбор флага `raw`.
5. В выпадающем списке `Text` выбирается `JSON`.
6. Сам текст (поле ниже):

```json
{
    "name": "Potion",
    "description": "Restores a small amount of HP",
    "price": 5
}
```

7. Send.

### Postman. Import from Swagger

Это слишком сложно, долго и нудно вот так вот вручную вводить запросы в Postman. Их можно
импортировать из Swagger.

1. Открываем стартовую страницу микросервиса в браузере: `https://localhost:5001/swagger/index.html`.
2. Копируем адрес ссылки с открывшейся страницы `https://localhost:5001/swagger/v1/swagger.json`
в буфер обмена.
3. В Postman нажимаем "Import" -> "Link" -> вставляется линк -> "Import".

3 пункт через Link не получилось импортировать. Postman пишет: "error while fetching data from link".

Получилось импортировать через:
"Import" -> "Raw text" -> Вставить в виде текста содержимое `swagger.json` -> "Import".

4. В Postman, во вкладке "Collections" появляются импортированные запросы.
5. Адреса в импортированных запросах выглядят подобным образом: `{{baseUrl}}/items`.
6. Задание значения для `{{baseUrl}}`:
   1. Перейти на верхнюю папку коллекции запросов и выбрать `...` (View more actions).
   2. Edit -> Variables
   3. В столбцах "Initial value" и "Current value" для переменной "baseUrl" задать адрес запущенного
   микросервиса: `https://localhost:5001`.
   4. Update

Теперь можно пробовать посылать запросы к API через Postman.

### Postman. Export collection, History, Environment, 

**Export collection**:

1. Перейти на верхнюю папку коллекции запросов и выбрать `...` (View more actions).
2. Export

**History** (Закладка слева). Содержит список запросов, которые были выполнены ранее.

**Environment** (комбобокс справа вверху). Позволяет переключать конфигурации запросов.

**Authorization** (Закладка). Позволяет сгенерировать запрос для авторизации (разные виды) через API.

### Отключение автоматического открытия броузера при запуске .NET Core приложения

1. Файл `.vscode/launch.json` -> секция `serverReadyAction`
2. Если удалить эту секцию, то браузер прекратит автоматически открываться при запуске приложения.
Тем не менее, микросервис все равно будет запускаться.

## Lesson 23. Reusing common code via NuGet

* Don't Repeat Yourself (DRY). Надо вынести общий код для всех микросервисов в отдельное, доступное
для них место.
* Microservices should be independent of each other. Нельзя оставить общий код в одном микросервисе
и сослаться на него из другого микросервиса.
* Each microservice should live in its own source control repository. И положить библиотеку
с общим кодом в виде проекта рядом с микросервисами тоже не получится.
* Решение - NuGet.

Немного про NuGet:

* NuGet is the package manager for .NET.
* A NuGet package is a single ZIP file (.nupkg) that contains files to share with others.
* Microservice projects don't need to know where NuGet packages are hosted.
* The common code is now maintained in a single place.
* The time to build new microservices is significantly reduced.

## Lesson 26. Moving generic code into a reusable NuGet package

Используется отдельная директория `Play.Common`. Лежит по соседству с другими директориями
микросервисов.

Создание библиотеки:

```text
dotnet new classlib -n Play.Common
```

После создания нового проекта проверяем запущен ли OmniSharp сервер в VS Code: для этого отрываем
любой `*.cs` файл в проекте.

После, в Command Palette выбираем: ".NET Generate Assets for Build and Debug". Это сгенерирует
директорию `.vscode` с `tasks.json` (сгенерится в директории, откуда запущен VS Code).

В `tasks.json` добавляем:

```json
"tasks": [
{
    //...
    "args": [ //...
    ],
    "problemMatcher": "$msCompile",    // Добавить под эту строку
    "group": {
        "kind": "build",
        "isDefault": true
    }
},
```

Добавление nuget-пакетов:

```text
dotnet add package MongoDB.Driver
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Binder
dotnet add package Microsoft.Extensions.DependencyInjection
```

### Создание nuget-пакета

```text
dotnet pack -o ../../../packages
```

где `-o` - выходная директория.

### Добавление в проект источника nuget-пакетов (локальная директория)

```text
dotnet nuget add source /полный_путь/packages -n PlayEconomy
```

где `n` - имя источника nuget-пакетов.

В Linux источник nuget-пакетов добавляется сюда: `/home/USER/.nuget/NuGet/NuGet.Config`.
`

## Lesson 27. Introduction to Docker Compose

Why need yet another Docker tool?

1. Multiple docker container to run.
2. Too many steps to setup infrastructure services.
3. Too many arguments to remember.
4. Some containers may need to talk to each other.
5. What if container depends on another container?

### What is Docker Compose?

A tool for defining and running multi-container Docker applications.

1. Для конфигурации docker compose используется файл `docker-compose.yml`.
Этот файл содержит информацию о:

* Запускаемых контейнерах
* Переменных окружения
* Порты
* Зависимости между контейнерами

2. Одна команда для запуска: `docker-compose up`.

3. Также docker compose предоставляет *Compose network*, благодаря которой контейнеры могут
общаться друг с другом.

## Lesson 28. Moving MongoDB to docker compose

Включение показа пробелов в VS Code:

```text
File -> Preferences -> Settings ->
-> Editor: Render Whitespace (поиск по "render whitespace") -> all
```

Установка extension для VS Code `Docker` (by Microsoft).

В отдельной (соседней) директории (Play.Infra) создается файл `docker-compose.yml`.

### В docker-compose.yml

(Пробельные отступы для задания свойств внутри секций обязательны - рекомендуется включить
в IDE показ пробелов).

1. Задание *version*. Version определяет какие features будут доступны для docker compose engine.

```yml
version: "3.8"      # На linux у меня запустилась только версия "3.3"
```

2. Секция *services*. Задает docker container. На примере container для mongodb.

Напоминание. Для запуска под docker-контейнером использовалась следующая команда:

```text
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo:4.4.7
```

```yml
services:                             # секция
    mongo:                            # имя сервиса 
        image: mongo:4.4.7            # имя image контейнера с версией (которое в конце команды)
        container_name: mongo         # отображаемое имя контейнера (которое --name)
        ports:
            - 27017:27017             # массив портов, каждый порт на отдельной строке
        volumes:
            - mongodbdata:/data/db    # массив volumes (один элемент)

volumes:
    mongodbdata:
```

**Примечания:**

1. Если для `image` версия не нужна, то она просто не указывается.

2. docker-compose создает новый экземпляр `volumes` при переходе с обычного запуска docker.
Поэтому все `volumes` при таком переходе становятся пустыми.

### Остановка контейнера docker

1. Смотрим запущен ли контейнер:

```text
docker ps
```

2. Остановка контейнера:

```text
docker stop mongo
```

где `mongo` - имя контейнера, которое устанавливается при помощи атрибута `--name`.

### Запуск docker compose

Из директории, где лежит файл `docker-compose.yml`.

```text
docker-compose up -d
```

* `-d` (detach) - запуск docker-compose в "backround" режиме (в консоль не выводятся логи работы).
