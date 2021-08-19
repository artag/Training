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

## Lesson 30. Creating the Inventory microservice

### В VS Code

1. В терминале VS Code. Переоткрытие VS Code в новом каталоге, в том же самом окне.

```text
code . -r
```

2. Создание нового микросервиса:

```text
dotnet new webapi -n Play.Inventory.Service
```

3. Открытие файла `Program.cs` в новом проекте, активация OmniSharp server в VS Code ->
автоматическое создание `.vscode` вместе с настройками запуска приложения.

4. Как обычно, в `.vscode/tasks.json` добавляется:

```json
"group": {
    "kind": "build",
    "isDefault": true
}
```

5. В `.vscode/launch.json` удаляется секция `serverReadyAction`.

6. В `Properties/launchSetting.json` для этого микросервиса поставим другие порты, чтобы
не пересекаться с соседним микросервисом (вместо 5001 и 5000):

```json
"applicationUrl": "https://localhost:5005;http://localhost:5004",
```

7. Добавление общего nuget-пакета в проект `Play.Inventory.Service`:

```text
dotnet add package Play.Common
```

8. Добавление:

* `Controllers/ItemsController.cs`
* `Entities/InventoryItem.cs`
* `Dtos.cs`
* `Extensions.cs`

9. В `appsettings.json` добавлются настройки `ServiceSettings` и `MongoDbSettings`
(по аналогии с `Play.Catalog.Service`):

```json
"ServiceSettings": {
  "ServiceName": "Inventory"
},
"MongoDbSettings": {
  "Host": "localhost",
  "Port": "27017"
},
```

10. В `Startup.cs`, в методе `ConfigureServices` регистрируем `MongoDB`:

```csharp
services.AddMongo()
        .AddMongoRepository<InventoryItem>("inventoryitems");
```

### В броузере

11. Запускаем микросервис, заходим по адресу `https://localhost:5005/swagger/index.html`.
Копируем сгенеренный файл `/swagger/v1/swagger.json` виде текста для импорта в Postman.

### В Postman

12. Импортируем сохраненный json. В Postman:

```text
File -> Import... -> Raw text -> вставка текста -> Continue
```

13. Определяем значение `{{baseUrl}}` в запросах:

```text
Название коллекции -> ... -> Edit -> Variables
В "Initial Value" и "Current Value" вставляем адрес микросервиса https://localhost:5005
```

### Генерация guid в запросе в Postman

В Postman есть встроенная функция `$guid` для генерации guid в body запроса:

```json
{
    "userId": {{$guid}},
    ...
}
```

Узнать сгенеренный guid можно после выполнения запроса в Console Postman (внизу окна):

```text
Console -> Развернуть выполненный запрос (в видео это Post) -> Request Body -> нужный guid.
```

## Lesson 31. Introduction to synchronous communication

2 способа коммуникации между сервисами:

* *Synchronous* - The client sends a request and waits for a response from the service.

* *Asynchronous* - The client sends a request to the service but the response, if any, is not
sent immediately.

### Synchronous communication style

* The client sends a request and waits for a response from the service.

* The client cannot proceed without the response.

* The client thread may use a blocking or non-blocking implementation (callback).

* REST + HTTP protocol is the traditional approach.

* gRPC is an increasingly popular approach for internal inter-service communication.

## Lesson 32. Implementing synchronous communication via IHttpClientFactory

1. Проблема предыдущей реализации Inventory service. Для пользователя возвращаются предметы,
но о предметах информация представлена в виде `catalogItemId` и `quantity`.

2. Вся информация о предмете содержится в Catalog service

3. Inventory service должен обратиться к Catalog service, чтобы считать по `catalogItemId`
свойства предмета (`name` и `description`) и сформировать более полный ответ.

### Реализация

1. Алгоритм работы на сервисе Play.Inventory такой:

```text
1) В ItemsController приходит запрос GetAsync(Guid userId)

2) Из каталога считываются через HttpClient все item'ы:
_catalogClient.GetCatalogItemsAsync() -> IReadOnlyCollection<CatalogItemDto>
где CatalogItemDto содержит: Guid Id, string Name, string Description

3) Из БД считываются все предметы, принадлежащие определенному user:
_itemsRepository.GetAllAsync(item => item.UserId == userId) -> IReadOnlyCollection<InventoryItem>
где InventoryItem содержит: Guid Id, Guid UserId, Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate.

4) Для каждого InventoryItem из IReadOnlyCollection<InventoryItem> выбирается CatalogItemDto
и из них создается InventoryItemDto,
где InventoryItemDto содержит: Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate.
```

2. Для синхронной связи между сервисами будет использоваться `HttpClient`
(см. `Play.Inventory.Service/Clients/CatalogClient.cs`).

3. Регистрация `CatalogClient` в `Startup.ConfigureServices`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddHttpClient<CatalogClient>(client =>
    {
        // Адрес другого микросервиса (для связи).
        client.BaseAddress = new Uri("https://localhost:5001");
    });
    // ...
}
```

### Проблемы с ошибкой SSL certificate в HttpClient

У меня, в реальности, запрос по `HttpClient` не проходил - вылетало исключение при попытке сделать
запрос на соседний микросервис из `CatalogClient`:
что-то типа SSL certificate of that site comes from untrusted site.

#### Решение 1. Отключение проверки SSL certificate

Эту проверку можно отключить при регистрации `CatalogClient` в `Startup.ConfigureServices`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddHttpClient<CatalogClient>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:5001");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return  new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =     // эта строка отключает
            (message, cert, chain, errors) => true          // проверку SSL certificate
        };
    });
    // ...
}
```

#### Решение 2. Шаманство в Linux. Не проверено

Ссылки от лектора:

* [How to run 'dotnet dev-certs https --trust'?](https://stackoverflow.com/a/59702094/6105076)

* [Running API and MVC projects SSL connection could not be established](https://forums.asp.net/t/2174270.aspx?Running+API+and+MVC+projects+SSL+connection+could+not+be+established)

#### 1. Ответ от некоего Fa

*First*: modifying the `Main` method via:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
            .UseKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 5004);
                options.Listen(IPAddress.Loopback, 5005,
                    listenOptions =>
                    {
                        listenOptions.UseHttps("/home/https/localhost.pfx", "******");
                    });
            })
            .UseStartup<Startup>(); 
        });
}
```

*Second*: I added the `certificate.pem` file to Postman from the settings then certificate section.

#### 2. Ответ от некоего Neas

I also had this issue on Ubuntu 20.04;

I found Fa solution a bit vague and couldn't get it to work.
I can see how it should work though but being somewhat naive with respect to ssl certificate
and https workflow still had the issue described.

I found the following How to run
[How to run 'dotnet dev-certs https --trust'?](https://stackoverflow.com/a/59702094/6105076)
detailed solution useful in creating pfx and pem.

Then similar the following changes to both `Play.Catalog.Service` and
`Play.Inventory.Service` `Program.cs`:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            // Added this section to read kestrel options
            services.Configure<KestrelServerOptions>(
                context.Configuration.GetSection("Kestrel"));
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            // webBuilder.UseUrls("http://*:5000", "https://*:5001");
        });
```

## Lesson 33. Understanding timeouts and retries with exponential backoff

Но микросервис "Catalog" может быть иногда недоступен для микросервиса "Inventory".

In a distributed system, whenever a service makes a synchronous request to another service,
there is an *ever-present* (постоянный) risk of partial failure.

Возможные причины такой недоступности:

* Network outages (перебои в работе сети)
* Hardware failures
* Dependency failure
* Deployment in-progress

You must design your service to be resilent (невосприимчивый) to those partial failures.

### Setting timeouts

A service client should be designed not to block indefinitely and use timeouts.

```text
                                     Timeout: 1 sec
       --------->                   ---------------->
Client            Inventory Service                   Catalog Service
       Fail Fast                        Fail Fast
       <---------                   <---------------
```

**Совет**: use timeouts for a more responsive experience and to ensure resources are never tied up
indefenitely.

### Retries with exponential backoff

Performs call retries a certain number of times with a longer wait between each retry.

```text
       ------->                   ------------->
Client          Inventory Service  Wait 2 secs   Catalog Service
                                  ------------->
                                   Wait 4 secs   
                                  ------------->
                                      ...
        Fail                          Fail
       <-------                   <-------------
```

### Для тестирования задержек при синхронном общении Play.Inventory и Play.Catalog

В `Play.Catalog.ItemsController`, метод `GetAsync` добавляется искусственные задержки с целью
отладки обработки задержек передач в `Play.Inventory`:

```csharp
private static int requestCounter = 0;

[HttpGet]
public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
{
    // Для тестирования обработки Timeout и прочих ошибок соединения на другой стороне
    requestCounter++;           // Счетчик для эмуляции разного поведения ответа сервиса
    Console.WriteLine($"Request {requestCounter}: Starting...");

    if (requestCounter <= 2)
    {
        Console.WriteLine($"Request {requestCounter}: Delaying...");
        await Task.Delay(TimeSpan.FromSeconds(10));
    }

    if (requestCounter <= 4)
    {
        Console.WriteLine($"Request {requestCounter}: 500 (Internal Server Error).");
        return StatusCode(500);
    }

    // Полезная логика
    var items = (await _itemsRepository.GetAllAsync())
        .Select(item => item.AsDto());

    Console.WriteLine($"Request {requestCounter}: 200 (OK).");
    return Ok(items);
}
```

## Lesson 34. Implementing a timeout policy via Polly

Добавляется Fail Fast поведение в работу микросервиса `Play.Inventory`.
Добавляется Timeout к соединению по http с сервисом `Play.Catalog`: если последний не отвечает
по истечении Timeout, то `Play.Inventory` отключается от соединения, не дожидаясь его окончания.

1. Add nuget package `Polly` в `Play.Inventory`.
Polly позволяет to properly handle transient errors in an easy way.

```text
package add package Microsoft.Extensions.Http.Polly
```

2. Добавление конфигурации для `HttpClient` в `Startup.ConfigureServices`:

*(Порядок определения/задания правил для HttpClient важен)*

```csharp
services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    // Мое добавление для отключения проверки сертификата (см. пред. уроки).
})
// Добавление timeout в секундах (1 секунда в примере)
// По превышении timeout кидает TimeoutRejectedException.
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1))
```

## Lesson 35. Implementing retries with exponential backoff

Для включения повторных попыток связи, в `Play.Inventory` для `HttpClient` в
`Startup.ConfigureServices` добавляется `AddTransientHttpErrorPolicy` с настройкой
`WaitAndRetryAsync`:

*(Порядок определения/задания правил для HttpClient важен)*

```csharp
 var jitterer = new Random();    // Для рандомизации времени попытки доступа.

services.AddHttpClient<CatalogClient>(client =>
{
    // ..
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    // ..
})
// (1) AddTransientHttpErrorPolicy добавляется до AddPolicyHandler
// (2) Specifies the type of exception that this policy can handle.
// (3) Количество повторов соединения
// (4) Время между повторами. Каждый раз увеличивается в 2 раза.
// (5) Небольшой случайный разброс по времени нужен для сглаживания пиковой нагрузки на
//     сервис, если сразу несколько клиентов будут делать повторные запросы.
// (6) Делегат, срабатывающий при повторе. Логгер здесь достается через serviceProvider.
//     В production code так делать НЕ НАДО, только для учебного примера.
.AddTransientHttpErrorPolicy(builder =>                                     // (1)
    builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(               // (2)
        retryCount: 5,                                                      // (3)
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +   // (4)
                        TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),  // (5)
        onRetry: (outcome, timespan, retryAttempt) => {                     // (6)
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ILogger<CatalogClient>>()?
                .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, than making retry {retryAttempt}");
        }
    ))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
```

В справке описано, что `AddTransientHttpErrorPolicy` обрабатывает следующие виды ошибок:

* Network failures (as System.Net.Http.HttpRequestException)
* HTTP 5XX status codes (server errors)
* HTTP 408 status code (request timeout)

## Lesson 36. Understanding the circuit breaker pattern

Политика повторов хорошая вещь, но всегда стоит помнить об ограниченности сервисных ресурсов.

Если сервис недоступен по каким-либо причинам, то его клиент(ы) постоянно пытается достучаться
до этого сервиса. Но ресурсы сервиса (потоки) ограничены. При таком сценарии вполне
вероятно столкнуться с *Resource Exhaustion* (исчерпанием ресурсов) на стороне клиента.

Когда это происходит, то этот клиент-сервис становится целиком недоступным для других
компонентов (служб/приложений) системы.

Для борьбы с *Resource Exhaustion* на клиенте можно применить circuit breaker pattern.

### Circuit breaker pattern (схема автоматического выключателя)

* Prevents the service from performing an operation taht's likely to fail
(предотвращает выполнение службой операции, которая может привести к сбою).

* Prevents our service from reaching resource exhaustion (исчерпание ресурсов).

* Avoids overwhelming the dependency (позволяет избежать чрезмерной зависимости).

Все запросы от сервиса-клиента посылаются через circuit breaker к сервису-серверу.
Circuit breaker следит за ошибками соединений.

Когда количество ошибок превышает предварительно заданный предел (threshold), circuit breaker
прерывает попытки соединения - переходит в состояние *Open circuit*.

В режиме "Open circuit" все запросы от клиента (Client) будут игнорироваться в течение заданного
периода времени. После окончания этого периода ожидания circuit breaker вновь позволяет запросу пройти
на сервер - circuit breaker переходит в состояние *Close circuit*.

```text
Client -----> Inventory Service -> Circuit breaker -----> Catalog Service
                 (Client)                           ....     (Server)
```

## Lesson 37. Implementing the circuit breaker pattern

Для включения circuit breaker, в `Play.Inventory` для `HttpClient` в
`Startup.ConfigureServices` добавляется `AddTransientHttpErrorPolicy` с настройкой
`CircuitBreakerAsync`:

*(Порядок определения/задания правил для HttpClient важен)*

```csharp
services.AddHttpClient<CatalogClient>(client =>
{
    // ... задается адрес сервера для соединения по http
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    // ... отключение проверки сертификата безопасности при соединении
})
.AddTransientHttpErrorPolicy(builder =>
    builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
        // ... задание повторов соединения
    )
)
// (1) AddTransientHttpErrorPolicy добавляется до AddPolicyHandler
// (2) Включение режима Circuit Breaker
// (3) Кол-во попыток до того как Circuit Breaker перейдет в режим "open circuit"
// (4) Время разрыва цепи.
//     Время, в течение которого не будет никакой реакции на запросы со стороны клиента.
// (5) Функция, которая выполняется, когда circuit opens
// (6) Функция, которая выполняется, когда связь с сервером восстанавливается.
//     Режим "close circuit"
// (7) Логгер здесь достается через serviceProvider.
//     В production code так делать НЕ НАДО, только для учебного примера.
.AddTransientHttpErrorPolicy(builder =>                                     // (1)
    builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(             // (2)
        handledEventsAllowedBeforeBreaking: 3,                              // (3)
        durationOfBreak: TimeSpan.FromSeconds(15),                          // (4)
        onBreak: (outcome, timespan) =>                                     // (5)
        {
            var serviceProvider = services.BuildServiceProvider();          // (7)
            serviceProvider.GetService<ILogger<CatalogClient>>()?
                .LogWarning($"Opening the circuit for {timespan.TotalSeconds} seconds...");
        },
        onReset: () =>                                                      // (6)
        {
            var serviceProvider = services.BuildServiceProvider();          // (7)
            serviceProvider.GetService<ILogger<CatalogClient>>()?
                .LogWarning($"Closing the circuit...");
        }
    )
)
// Задание timeout для соединения с сервером.
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));     
```

В режиме "open circuit" выкидывается исключение `BrokenCircuitException`, которое прилетает
в клиент (в примере - Postman).

Но, если через некоторое время снова сделать зарос с клиента (Postman) не перезагружая сервисы,
то запрос проходит нормально.

## Lesson 39. Introduction to asynchronous communication

*Service Level Agreement* (SLA) - a commitment (обязательство) between a service provider
and a client.

### The problem with synchronous communication

* Increased latency (если запрос от клиента идет через несколько промежуточных сервисов).
* Partial failure amplification (отказ одного промежуточного сервиса приводит к отказу всех
зависимых от него сервисов).
* Reduced SLA - если любой из сервисов имеет SLA 99.9% (обязательство - процент времени доступности
сервера). Несколько таких последовательно совязанных сервисов будут иметь более низкий SLA.

### Asynchronous communication style

* The client does not wait for a response in a timely manner.
* There might be no response at all (возможно ответа вообще не будет).
* Usually involves the use of a lightweight message broker. Брокер по натуре dumb и не содержит
бизнес логики. Его задача - передача сообщений от sender к receiver.
* Message broker has high availability.
* Messages are send to the message broker and could be received by:
  * A single receiver (asynchronous commands).
  * Multiple receivers (publish/subscribe events).

### Microservice autonomy

Микросервисы общаются друг с другом посредством сообщений только через Message Broker.

Если один из сервисов откажет, то работа других сервисов не прервется.

* Partial failures are not propagated (не распространяются).
* Independent service SLA. Для всех микросервисов SLA будет одинаковым.
* Microservice autonomy enforced (обеспечивается автономия микросервисов).

### Asynchronous propagation (распространение) of data

Если у одного из микросервисов обновятся данные в БД, то для другого микросервиса через
Message Broker можно также обновить данные.

Все нужные данные можно своевременно подтягивать в требуемый микросервис. Для получения
всех данных будет достаточно только одного запроса к этому микросервису.

* Data is eventually consistent (согласование данных).
* Preserves microservice autonomy (сохранение автономии микросервисов).
* Removes inter-service latency (устранение/уменьшение задержек между службами).

### Что будет использоваться

*RabbitMQ* - lightweight message broker that supports the AMQP protocol.

Работа с сообщениями в RabbitMQ напоминает почтовый ящик. Когда микросервис посылает сообщение
в RabbitMQ, оно попадает в *Exchange*. Благодаря определенной привязки (*binding*) сообщение
переправляется в нужный *Queue*, откуда RabbitMQ доставляет сообщение адресату(ам).

С целью "отвязки" нашего кода от RabbitMQ используется MassTransit.
*MassTransit* - distibuted application framework for .NET, поддерживает несколько message broker,
и упрощает конфигурирование и взаимодействие с ними.

## Lesson 40. Defining the message contracts

Добавим новую библиотеку в `Play.Catalog`:

```text
dotnet new classlib -n Play.Catalog.Contracts
```

И сделаем на нее reference в `Play.Catalog.Service`:

```text
dotnet add reference ../Play.Catalog.Contracts/Play.Catalog.Contracts.csproj
```

Добавим контракты в `Play.Catalog.Contracts` - `Contracts.cs`.
