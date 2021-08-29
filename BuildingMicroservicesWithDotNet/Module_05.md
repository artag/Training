# Module 5. Synchronous inter-service communication

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

### В браузере

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
