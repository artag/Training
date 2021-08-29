# Module 3. Adding database storage

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
