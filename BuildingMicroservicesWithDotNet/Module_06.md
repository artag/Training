# Module 6. Asynchronous inter-service communication

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

Добавим контракты в `Play.Catalog.Contracts` - класс `Contracts.cs`.

## Lesson 41. Publishing messages via MassTransit

### Nuget пакеты для MassTransit

Добавление nuget-пакетов в `Play.Catalog.Service`:

```text
dotnet add package MassTransit.AspNetCore
dotnet add package MassTransit.RabbitMQ
```

### Добавление и использование IPublishEndpoint в контроллере

В `ItemsController` добавляется `IPublishEndpoint` из MassTransit. В методах, где операции
изменения/модификации (POST, DELETE, UPDATE), добавляется (например):

```csharp
_publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));
```

### Конфигурация

1. В `appsettings.json` добавляется секция (пока временно, потом будет изменено):

```json
  "RabbitMQSettings": {
      "Host": "localhost"    // Где находится RabbitMQ
  },
```

2. Директория `Settings`, класс `RabbitMQSettings` (для использования в `Startup`).

3. Класс `Startup`, метод `ConfigureServices` регистрация MassTransit:

```csharp
//...
services.AddMassTransit(x =>    
{
    // Задание транспорта, который будет использоваться (RabbitMQ)
    x.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMQSettings = Configuration
            .GetSection(nameof(RabbitMQSettings))
            .Get<RabbitMQSettings>();

        configurator.Host(rabbitMQSettings.Host);       // Установка параметра host

        configurator.ConfigureEndpoints(
            context,
            new KebabCaseEndpointNameFormatter(_serviceSettings.ServiceName, includeNamespace: false));
    });
});

// To start MassTransit service. This service starts RabbitMQ bus.
services.AddMassTransitHostedService();
//...
```

4. (Необязательный шаг). В `appsettings.Development.json` меняется настройка уровня логирования:

```json
"LogLevel": {
    "Default": "Information",
    // ..
```

на

```json
"LogLevel": {
    "Default": "Debug",
    || ..
```

Уровень логирования изменен для изучения, что происходит на заднем плане, когда сервисы передают
друг другу сообщения.

## Lesson 42. Standing up a RabbitMQ docker container

### Добавление RabbitMQ в Docker Compose

В `docker-compose.yml` добавляется конфигурация для RabbitMQ container:

```yml
services:
  # ..
  rabbitmq:
    image: rabbitmq:management
    container_name: rabbitmq
    ports:
      - 5672:5672     # To publish and consume messages from RabbitMQ
      - 15672:15672   # To access to the RabbitMQ portal
    volumes:
      - rabbitmqdata:/var/lib/rabbitmq
    hostname: rabbitmq    # Если не будет указано, то каждый раз при перезапуске RabbitMQ
                          # будет брать random name, и будет internally сохранять данные
                          # в разных местах
  #..

volumes:
  # ..
  rabbitmqdata:       # To store RabbitMQ messages
```

### Запуск контейнеров и проверка работы

```text
docker-compose up -d
docker ps
```

#### Запуск RabbitMQ Management

Адрес web client'а: `localhost:15672`, логин/пароль: `guest` / `guest`

## Lesson 43. Refactoring MassTransit configuration into the reusable NuGet package

Перенос MassTransit из `Play.Catalog` в общий проект `Play.Common`.

### Изменения в `Play.Common`

1. Перенос из `Play.Catalog.Service.Settings` класса `RabbitMQSettings` в
`Play.Common.Settings`

2. Добавление nuget-пакетов в `Play.Common`:

```text
dotnet add package MassTransit.AspNetCore
dotnet add package MassTransit.RabbitMQ
```

3. Перенос конфигурации MassTransit из `Play.Catalog`, метода `Startup.ConfigureServices` в
`Play.Common.MassTransit`, статический класс `Extensions` (аналогично как для MongoDB).

Плюс добавление consumers registration для обработки сообщений из RabbitMQ.
Этого не было в `Play.Catalog`, но будет использоваться в `Play.Inventory`.
Есть несколько способов регистрации consumers, в видео рассмотрен способ регистрации через
`Assembly`'s to scan for consumers.

Вот что было добавлено в `Play.Common.MassTransit`, класс `Extensions`:

```csharp
services.AddMassTransit(configure =>
{
    // Регистрация consumers. Adds all consumers in the specified assemblies.
    configure.AddConsumers(Assembly.GetEntryAssembly());

    // Задание транспорта, который будет использоваться (RabbitMQ)
    configure.UsingRabbitMq((context, configurator) =>
    {
        var configuration = context.GetService<IConfiguration>();
        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMQSettings.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, includeNamespace: false));
    });
});

// To start MassTransit service. This service starts RabbitMQ bus.
services.AddMassTransitHostedService();
```

4. Создание nuget-пакета.

На этот раз при создании nuget-пакета укажем номер версии. Если не указать (как в прошлый раз),
то по умолчанию выставляется номер версии `1.0.0`.

```text
dotnet pack -p:PackageVersion=1.0.1 -o ../../../packages
```

где `-o` - выходная директория.

### Изменения в `Play.Catalog`

1. Удаление класса `RabbitMQSettings` в `Settings`.

2. В `csproj`:

* Удаление ссылок на пакеты `MassTransit.AspNetCore` и `MassTransit.RabbitMQ`.
* Поднятие версии `Play.Common` до `1.0.1`.

3. В `Startup` классе удаляются все перенесенные строки конфигурации MassTransit и
все удаленное заменяется добавлением одной строки:

```csharp
//..
// (1) - Добавление конфигурации MassTransit из Play.Common.
services.AddMongo()
        .AddMongoRepository<Item>(collectionName: "items")
        .AddMassTransitWithRabbitMQ();          // (1)
//..
```

### Создание nuget-пакетов для `Play.Catalog.Contracts`

В директории Play.Catalog/src/Play.Catalog.Contracts создаем nuget package:

```text
dotnet pack -o ../../../packages/
```

Сервис `Play.Inventory` будет использовать данный пакет для consume the messages.

## Lesson 44. Consuming messages for eventual data consistency

Изменения в сервисе `Play.Inventory`.

1. Добавление в Entities `CatalogItem`.

Необязательно использовать/определять все поля/свойства сущности из одного микросервиса
для использования в другом. В частности, в `CatalogItem` нужны только лишь следующие свойства:

* `Id`
* `Name`
* `Description`

2. Добавление в проект `Play.Inventory` nuget-пакетов:

```text
dotnet add package Play.Catalog.Contracts       // Контракты сообщений для consume the messages
```

... и повышение версии пакета `Play.Common` с `1.0.0.` на `1.0.1`.

### Defining the consumers

В новом каталоге Consumers опеределяются consumers, по одному на каждую операцию, которая будет
происходить в `Play.Catalog`: create, update and delete:

* `CatalogItemCreatedConsumer`
* `CatalogItemUpdatedConsumer`
* `CatalogItemDeletedConsumer`

1. Все эти классы реализуют интерфейс `IConsumer<T>`, где `T` - тип сообщения, который handle
(берется из nuget пакета `Play.Catalog.Contracts`).

2. Для обработки сообщений в этих классах используется storage in our local catalog items database -
инжектируем в конструкторы классов `IRepository<CatalogItem>`.

3. Для всех классов реализуем метод `Consume(ConsumeContext<T> context)`.

### Добавление RabbitMQ в `appsettings.json`

(Такие же строки, как и для `appsettings.json` в `Play.Catalog`)

```json
// ..
  "RabbitMQSettings": {
      "Host": "localhost"
  },
//..
```

### Измененения в `Startup`

1. Добавление новой коллекции `CatalogItem` для хранения в MongoDB.
И регистрация в MassTransit, RabbitMQ.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ..
    services.AddMongo()
            .AddMongoRepository<InventoryItem>("inventoryitems")
            .AddMongoRepository<CatalogItem>("catalogitems")        // добавление
            .AddMassTransitWithRabbitMQ();                          // добавление
    // ..
}
```

Обе коллекции хранятся в одной БД.

### Необязательный шаг

В `appsettings.Development.json` меняется настройка уровня логирования:

```json
"LogLevel": {
    "Default": "Information",
    // ..
```

на

```json
"LogLevel": {
    "Default": "Debug",
    || ..
```

Уровень логирования изменен для изучения, что происходит на заднем плане, когда сервисы передают
друг другу сообщения.

### Перед запуском сервисов. Чистка БД

Перед запуском сервисов в обновленной конфигурации (добавление синхронизации БД через
шину сообщений) необходимо почистить БД у обоих сервисов, т.к. они находятся в
несинхронизированном состоянии.

Для этого в VSCode переключаемся на extension `MongoDB for VS Code`, делаем ПКМ на нужной
базе -> команда "Drop Database...".

## Lesson 45. Remove the inter-service synchronous communication. Enable RabbitMQ message retries.

Удаление синхронного взаимодействия `Play.Inventory` <-> `Play.Catalog`. Теперь ненужно, т.к.
вся информация о `CatalogItem` есть у сервиса `Play.Inventory` в его БД.

Все изменения производятся в сервисе `Play.Inventory`.

1. Удаление синхронного взаимодействия из `Controllers.ItemsController`.
    * Удаление поля (и его инициализации) `CatalogClient`.
    * Добавление `IRepository<CatalogItem>`.
    * Модификация метода `GetAsync`.

### Enable RabbitMQ message retries

Настройка повторных посылок сообщений в конфигурации RabbitMQ. Если вдруг consumer
не смог по какой-либо причине обработать сообщение.

Настройка в сервисе `Play.Common`, `MassTransit`, класс `Extensions`, метод расширение
`AddMassTransitWithRabbitMQ`:

```csharp
public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
{
    services.AddMassTransit(configure =>
    {
        // ..

        // Задание транспорта, который будет использоваться (RabbitMQ)
        configure.UsingRabbitMq((context, configurator) =>
        {
            // ..
            configurator.UseMessageRetry(retryConfigurator =>
            {
                retryConfigurator.Interval(retryCount: 3, interval: TimeSpan.FromSeconds(5));
            });
        });
    });
}
```

* `retryCount` - количество повторов сообщений
* `interval` - интервал между сообщениями

#### Обновление nuget-пакета. (Напоминание)

```text
dotnet pack -p:PackageVersion=1.0.2 -o ../../../packages
```

*Примечание*: PackageVersion - это версия nuget пакета, сама сборка подписывается как 1.0.0.
Чтобы подписать сборку, надо заводить либо `AssemblyInfo.cs` файл, либо добавлять
соответствующие атрибуты в файл проекта `Play.Common`.

### Проверка взаимодействия в Postman. (Напоминание)

1. Генерация произвольного user guid в запросе Post - в теле запроса пишется `{{$guid}}`:

```json
{
    "userId": "{{$guid}}",
    "catalogItemId": "d06ca638-0213-4a58-9fee-77566d514704",
    "quantity": 1
}
```

2. Узнать какой user guid был сгенерирован в ходе запроса Post.

После запроса Post, внизу экрана в Postman:

```text
Console -> Раскрыть последний запрос Post -> Раскрыть Request Body -> Поле userId
```
