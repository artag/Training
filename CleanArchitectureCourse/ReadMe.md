# Чистая архитектура на практике

Курс: `https://www.udemy.com/course/clean-architecture-csharp-ru/`

Git: `https://github.com/denis-tsv/CleanArchitectureCourse`

## Создание Migration

### Установка (обновление) Entity Framework Core Tool

Надо сделать только раз. Либо шаг 1, либо шаг 2.

Шаг 1. Если `dotnet-ef` еще не установлен

```text
dotnet tool install --global dotnet-ef
```

Шаг 2. Если `dotnet-ef` еще не установлен, но требует обновления:

```text
dotnet tool update --global dotnet-ef
```

### Создание миграции

```text
dotnet ef migrations add Initial -s WebApp.csproj -p ../DataAccess/DataAccess.csproj
```

- `Initial` - имя миграции

- `-s WebApp.csproj` - The startup project to use.
  Defaults to the current working directory.

- `-p ../DataAccess/DataAccess.csproj` - The project to use. Defaults to the current
  working directory. (Именно здесь и будет создана migration).

### Применение миграции

```text
dotnet ef database update -v
```

- `-v` или `--verbose` - необязательный параметр, позволяет увидеть более подробную информацию
  о том, что происходит при выполнении команды.

Файл БД `WebApp.db` появляется прямо в директории `01. Initial/WebApp/`.

## Проект 01. Начальный проект

Проект `01. Initial` - начальный проект по "стандартной" архитектуре.

Его схема:

<img src="images/01_demo_project.jpg" alt="Demo project" style="width:450px">

## Cross-cutting concerns. Helpers

*Проект: 02. AddUtilsAndDomainServices*

На диаграммах чистой и луковой архитектур обычно не указывается область cross-cutting concerns.

**Cross-cutting concerns** - это сквозная (или пронизывающая) функциональность.

- Относится к инфраструктуре приложения.
- Часто используется несколькими слоями приложения.

Примеры cross-cutting concerns:

- Транзакционность
- Логирование
- Измерение производительности
- Extension методы
- Атрибуты
- Helpers

В примере в solution добавится проект `Utils`. Там будет простые методы расширения для `string`.

На этот проект могут ссылаться все остальные проекты в solution: добавляем ссылку на проект `Utils`
в `Domain`.

Луковая архитектура без `Utils`:

<img src="images/02_without_utils.jpg" alt="Without utils" style="width:450px">

Луковая архитектура с `Utils`:

<img src="images/03_with_utils.jpg" alt="With utils" style="width:450px">

## Entities

- Это бизнес-логика, которая существует в реальной жизни (без софта).
- Никакой больше логики кроме бизнес-логики в Entities быть не должно. Никаких интерфейсов,
репозиториев и т.п.
- Рекомендуется использовать Rich-model (дядя Боб рекомендует).
- Но в реальной жизни анемичная модель также используется.

Примеры бизнес-логики, которая может существовать в реальной жизни и без софта:

- Магазин: рассчет стоимости заказа, с учетом/без учета скидки.
- Бухгалтерия: рассчеты в тетрадях и т.п.

**Rich-model** - модели, которые содержат не только данные, но и инкапсулируют методы для их
обработки.

**Анемичная модель** - модель, которая содержит только данные.

### Что положить в Entities

- Entities (модели) - сущности, которые моделируют объекты предметной области.
- Перечисления
- Исключения - только исключения, относящиеся к предметной области.
- Domain Events

Пример entities для интернет магазина:

- Заказ
- Товар
- Пункт заказа

Пример исключений, относящихся к предметной области:

- НельзяРассчитатьСтоимостьЗаказа
- БалансНеСошелся

### Если будет использоваться анемичная модель

Для этого надо выделить Domain Services для бизнес-логики.

В демо-проекте будет продемонстрирован пример использования анемичной модели.

В solution добавляются проекты:

- `DomainServices.Interfaces` - интерфейсы
- `DomainServices.Implementation` - реализация интерфейсов

<img src="images/04_add_domain_services.jpg" alt="Add domain services" style="width:500px">

Кто может ссылаться:

- На `DomainServices.Interfaces` могут ссылаться все вышестоящие проекты.
- На `DomainServices.Implementation` может ссылаться самый верхний "root" проект:
  - На рисунке это `Frameworks`
  - В демо-проекте это запускаемый верхний проект `WebApp`.

### Практика

Перенос метода расчета стоимости заказа из `Application.OrderService` в слой Entities.

```csharp
public async Task<OrderDto> GetByIdAsync(int id)
{
    // ...
    dto.Total = order.Items.Sum(x => x.Quantity * x.Product.Price)
    return dto;
}
```

Если будет использоваться rich-model, то просто перенос в entities, в класс `Order`:

```csharp
public class Order
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public OrderStatus Status { get; set; }

    public ICollection<OrderItem> Items { get; set; }

    // NEW
    public decimal GetTotal()
    {
        return Items.Sum(x => x.Quantity * x.Product.Price);
    }
}
```

Но у нас используется анемичная model:

1. В новый проект `DomainServices.Interfaces` добавляется:

```csharp
public interface IOrderDomainService
{
    decimal GetTotal(Order order);
}
```

2. В новый проект `DomainServices.Implementation` добавляется реализация:

```csharp
public class OrderDomainService : IOrderDomainService
{
    public decimal GetTotal(Order order)
    {
        return order.Items.Sum(x => x.Quantity * x.Product.Price);
    }
}
```

3. Добавление ссылок в проекты:

  - В `Application` ссылка на `DomainServices.Interfaces`
  - В `WebApp` ссылка на `DomainServices.Implementation`

4. Регистрация нового сервиса в контейнере:

```csharp
builder.Services.AddScoped<IOrderDomainService, OrderDomainService>();
```

5. Использование зарегистрированного сервиса `IOrderDomainService` в `Application.OrderService`.

## Data Access

*Проект: 03. AddDataAccess*

### Use Cases и Data Access

- В Use Cases находится логика приложения - логика, которые связана с автоматизацией бизнес-процессов.
- Не должны зависеть от фреймворков (ORM, Web, ...).
- На каждую роль один use case.

Примеры:

- Взаимодествие с инфраструктурой.
- Информация о сеансе текущего пользователя.

Use Cases нужны интерфейсы инфраструктуры:

- База данных
- Интеграции (email-сервис)
- Хост (текущий пользователь и его права)

### Расположение интерфейсов инфраструктуры

В луковой архитектуре расположение интерфейсов четко определено:

<img src="images/05_onion_interfaces.jpg" alt="Interfaces in onion" style="width:550px">

Расположение интерфейсов по DDD:

<img src="images/06_ddd_interfaces.jpg" alt="Interfaces in DDD" style="width:500px">

Где расположены интерфейсы инфраструктуры в чистой архитектуре:

- Не в Entities (как это реализовано в DDD).
- Интерфейсы инфраструктуры располагаются в слое UseCases.

Наш пример в качестве инфраструктуры использует базу данных.

Поэтому создаем и добавляем слой `DataAccess.Interfaces` и его реализацию `DataAccess`:

<img src="images/07_add_dataaccess_interfaces.jpg" alt="Adding DataAccess.Interfaces" style="width:550px">

- Слой `DataAccess.Interfaces` помещается между слоями `Entities` и `Use Cases`.
- Слой `DataAccess.Interfaces` ссылается на `Entities`.
- Слой `DataAccess.Interfaces` используется слоем `Use Cases`.
- Реализация слоя `DataAccess.Interfaces` находится в слое `DataAccess`, который расположен
ближе всего к слою `Frameworks`.

### Практика

#### Добавление проекта `DataAccess.Interfaces`

1. Создание проекта `DataAccess.Interfaces`.

2. Добавление в `DataAccess.Interfaces` ссылки на `Entities` и `Microsoft.EntityFrameworkCore`.

3. В `DataAccess.Interfaces`, создаем `IDbContext`:

```csharp
public interface IDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken token);
}
```

Почему в интерфейс доступа к БД мы добавляем `DbSet`, а не создаем репозитории?
Объяснение в двух словах:

- Мы здесь не зависим ни от какой конкретной базы (зависимость только от "общего" пакета
`Microsoft.EntityFrameworkCore`).

- Концепция `DbContext` сама по себе является реализацией паттерна `Repository` и `Unit of work`.
(Т.е. реализация дополнительной абстракции `Repository` является избыточной).

4. Добавление в `DataAccess` ссылки на `DataAccess.Interfaces`.

5. Класс `DataAccess.AppDbContext` теперь реализует `IDbContext` (добавляем `IDbContext`):

```csharp
public class AppDbContext : DbContext, IDbContext
{
    // ...
}
```

6. `Application` теперь ссылается не на `DataAccess`, а на `DataAccess.Interfaces`:

- Меняем ссылки в `.csproj`
- В классах, которые используют `AppDbContext` меняем на 

7. Верхний `WebApp` теперь ссылается на `DataAccess`.

- Меняем регистрацию сервиса:

с

```csharp
builder.Services.AddEntityFrameworkSqlite().AddDbContext<AppDbContext>();
```

на

```csharp
builder.Services.AddEntityFrameworkSqlite().AddDbContext<IDbContext, AppDbContext>();
```

#### Раскладывание проектов по папкам

Рекомендуется для сортировки проектов по уровням использовать именование папок с нумерацией.

Сортировка проектов по папкам в solution:

- Папка `0 Utils`:
  - Проект `Utils`
- Папка `1 Entities`:
  - Проект `Domain`
  - Проект `DomainServices.Implementation`
  - Проект `DomainServices.Interfaces`

### Итого

Слой `Application` теперь не зависит от типа БД, а зависит только от `IDbContext`.

## Много инфраструктуры

*Проект: 04. AddInfrastructure* - общий проект под инфраструктуру

*Проект: 05. AddInfrastructureSeparate* - отдельные проекты под инфраструктуру

Что делать если у приложения много инфраструктуры.

Например, в приложении может быть несколько вещей из:

- База данных (на чтение, на запись, KeyValue storage)
- Веб-фреймворк
- Service Bus
- Blob Storage (для хранения файлов)
- Elastic Search (для ускорения поиска)

### Что сделать

Выделить проект `Infrastructure.Interfaces` и положить туда интерфейсы всей инфраструктуры,
которая используется приложением:

<img src="images/08_add_infrastructure.jpg" alt="Add Infrastructure.Interfaces" style="width:650px">

- `DataAccess` лучше оставить отдельным слоем.
- В `Infrastructure.Implementation` будет находиться реализация остальной инфраструктуры.

`DataAccess` и `Infrastructure.Implementation` могут ссылаться **только** на
`Infrastructure.Interfaces` и, если это необходимо, на `Entities`.

### Что кладем в `Infrastructure.Interfaces`

- Интерфейсы для доступа к инфраструктуре
  - ORM для доступа к базе
  - Elastic Search
  - Blob Storage
  - ...
- Интерфейсы для зависимостей веб-фреймворка
  - Текущий пользователь (получение информации о текущем пользователе)
- Интерфейсы для интеграции с врешними системами
  - Отправка Email и SMS

#### Если инфраструктуры слишком много

То `Infrastructure.Interfaces` и `Infrastructure.Implementation` могут стать слишком большими.

Поэтому: выделяем интерфейс и реализацию каждой инфраструктуры.

Пример:

- Для Service Bus:
  - `ServiceBus.Interfaces`
  - `ServiceBus.Implementation`
- Для Blob Storage:
  - `BlobStorage.Interfaces`
  - `BlobStorage.Implementation`
- ...
- `Инфраструктура N`:
  - `Интеграция N.Interfaces`
  - `Интеграция N.Implementation`

<img src="images/09_add_infrastructure_2.jpg" alt="Add Infrastructure.Interfaces" style="width:650px">

При таком походе будет множество слабосвязанных маленьких компонентов.

### Практика

#### 1. Добавление только `Infrastructure.Interfaces` и `Infrastructure.Implementation`

*Проект: 04. AddInfrastructure*

1. `DataAccess.Interfaces` переименовывается в `Infrastructure.Interfaces`

2. В `Infrastructure.Interfaces` создаются папки:

- `Infrastructure` - для доступа к БД - сюда помещеается `IDbContext`.
- `Integrations` - для какого-нибудь email сервиса.
- `WebApp` - для получения информации о пользователе.

3. Интерфейсы для наших псевдо-сервисов:

- В `Infrastructure.Interfaces`, `Integrations`:
создадим `IEmailService` - интерфейс email сервиса.

- В `Infrastructure.Interfaces`, `WebApp`:
создадим `ICurrentUserService` - интерфейс сервиса для получения информации о текущем пользователе.

4. Новый проект `Infrastructure.Implementation` ссылается на `Infrastructure.Interfaces`.

5. Можно сгруппировать объекты по папкам:

- Папка `2 Infrastructure.Interfaces`:
  - Проект `Infrastructure.Interfaces`
- Папка `Infrastructure.Implementation`:
  - Проект `DataAccess`
  - Проект `Infrastructure.Implementation`

6. Добавление реализаций

- В `Infrastructure.Implementation` - реализацию для `IEmailService`.
- В `WebApp`, папку `Services` - реализацию для `ICurrentUserService`.

7. Слой `Application` ссылается на `Infrastructure.Interfaces`.

8. Слой `WebApp` ссылается на `Infrastructure.Implementation` и `DataAccess`.

9. Регистрация новых сервисов на `WebApp`:

```csharp
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
```

Можно сгруппировать регистрацию сервисов по слоям:

```csharp
// Domain
builder.Services.AddScoped<IOrderDomainService, OrderDomainService>();

// Infrastructure
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<IDbContext, AppDbContext>();

// Application
builder.Services.AddScoped<IOrderService, OrderService>();

// Frameworks
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddControllers();
```

#### 2. Добавление `Infrastructure.Interfaces` и `Infrastructure.Implementation` для каждого компонента инфраструктуры

*Проект: 05. AddInfrastructureSeparate*

Все сервисы инфраструктуры разбиваются на попарные проекты.

1. В solution получаются следующие компоненты инфраструктуры:

- `DataAccess.Interfaces` и `DataAccess`
- `Email.Interfaces` и `Email.Implementation`
- `Web.Interfaces` и `WebApp`

2. Добавление интерфейсов для наших псевдо-сервисов:

- В `Email.Interfaces` добавляем `IEmailService`
- В `Web.Interfaces` добавляем `ICurrentUserService`
- В `DataAccess.Interfaces` остается `IDbContext`

3. Интерфейсы инфраструктуры могут ссылаться только на "нижние" слои:

- `Entity`
- `Utils`

4. Релизации инфраструктуры

- `Email.Implementation`, добавляется `EmailService`
- `WebApp` - в папку `Services`добавим `CurrentUserService`
- `DataAccess.Interfaces` - лежит `AppDbContext`

5. Слой `Application` ссылается на интерфейсные слои инфраструктуры `*.Interfaces`

6. Слой `WebApp` ссылается на реализацию слоев инфраструктуры `*.Implementation`

7. Обязательно: регистрация в `WebApp` всех сервисов (см. прошлый раздел)

## Use Cases

*Проект: 06. UseCases*

Определения:

- **Use Case** - это операция, которую хочет выполнить пользователь.

- **Use Case** - вариант использования нашей системы.

Пользователь это не обязательно живой человек.

Примеры use case для интернет магазина:

- Оформить заказ
- Выбрать товар для просмотра по нему деталей
- Отменить заказ

Триггером use case может быть:

- Внешняя система, которая загружает или получает из нашей системы какие-либо данные.
- Background job (запуск по расписанию, синхронизация).

### Что такое интерактор

Дядя Боб обычно изображает секцию с Use Case так:

<img src="images/10_usecases_interactor.jpg" alt="Use Cases interactor" style="width:500px">

- `Controller` - отдает данные во входной порт Use Case (`Input Port`).
- `Presenter` - получает результаты работы Use Case из выходного порта (`Output Port`) и дальше
доставляет их пользователю.

Еще одна картинка от дяди Боба:

<img src="images/11_usecases_interactor_2.jpg" alt="Use Cases interactor 2" style="width:600px">

Из этих двух картинок видно что `Use Case Interactor`:

- Получает входные параметры/данные `Input Data`
- У него есть возможность взаимодействия с уровнем доступа к данным `Data Access Interface`
- У него есть возможность взаимодействия с `Entities`

Что делает Interactor: он управляет взаимодействием инфраструктуры с одной стороны (доступ к данным)
и бизнес-логика с другой стороны.

Например Interactor:

- загружает данные из базы
- выполняет бизнес-операции
- как-то изменяет объекты, загруженные в память
- сохраняет данные в базу
- может взаимодействовать с внешней системой: отправка SMS, email, сообщений по шине, ...

### Как реализовать Interactor

Логика уровня приложения, которая напрямую не связана с бизнес-логикой, а связана с ее
автоматизацией.

#### 1 способ реализации. Application Services

Обычно используются в слоистой архитектуре.

```csharp
public class OrderService
{
    public Order CreateOrder()
    {
    }

    public List<Order> GetOrders()
    {
        return _orderRepository.GetAllOrders();
    }
}
```

Здесь 2 метода - 2 use case'а.

#### 2 способ реализации. CQRS Handlers

Создается отдельный класс для каждого use case'а.

```csharp
public class GetOrderRequestHandler
{
    public List<Order> Handle(GetOrderRequest request)
    {
        return _dbContext.Orders;
    }
}
```

#### Какую из реализации выбрать? Сервис или хедлер?

- Правилу зависимостей удовлетворяют оба варианта.
- Автор рекомендует использовать хендлеры:
  - Явный вызов use case'а из use case'а.
  - Изоляция use case'ов.
  - Только необходимые зависимости и Generic-параметры для каждого из use case.
  - *(Что?)* AOP (aspect oriented programming) без магии Fody (pipeline).
  - *(Что?)* Cross-cutting concerns через pipeline, а не магию Fosy или DynamicProxy.

### Практика

*Проект: 06. UseCases*

1. Добавим еще один Use Case (для наглядности).

1.1. Сейчас есть только 1 use case: в `Application`, в `OrderService`, метод `GetByIdAsync`.

Добавим метод `CreateOrderAsync` в `IOrderService`:

```csharp
Task<int> CreateOrderAsync(CreateOrderDto dto);
```

И его реализацию в `OrderService`:

```csharp
public async Task<int> CreateOrderAsync(CreateOrderDto dto)
{
    var order = _mapper.Map<Order>(dto);
    _dbContext.Orders.Add(order);
    await _dbContext.SaveChangesAsync();
    return order.Id;
}
```

Dto'шки:

```csharp
public class CreateOrderDto
{
    public List<OrderItemDto> Items { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public decimal Total { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
```

Не забываем добавить Dto'шки в `MapperProfile`.

1.2. Новый метод в контроллере `OrdersController`:

```csharp
[HttpPost]
public async Task<int> Create([FromBody] CreateOrderDto dto)
{
    var id = await _orderService.CreateOrderAsync(dto);
    return id;
}
```

Теперь, переход к реализации Use Cases

2. Проект `Application` переименуем в `UseCases`.

3. Подключение `MediatR`.

- В проект `UseCases` добавляется nuget пакет `MediatR`.
- В проект `WebApp` добавляется nuget пакет `MediatR.Extensions.Microsoft.DependencyInjection`.

*Мое замечание: использование `MediatR` некоторые рассматривают как антипаттерн.*

4. Отрефакторим сервисы - заменим их на хендлеры.

4.1. Для каждого агрегата внутри проекта `UseCases` создадим отдельные папки.

- Папка `Order`. Внутри - все, что относится к заказу.
  - Папка `Order\Commands` - команды.
    - Папка `Order\Commands\CreateOrder` - Команда "Создание заказа".
  - Папка `Order\Queries` - queries.
    - Папка `Order\Queries\GetById` - Query "Получение заказа".
  - Папка `Order\Dto` - папка для Dto'шек.
  - Папка `Order\Utils` - папка для вспомогательных инструментов.

4.2. Создадим внутренний request, который отправляется от контроллера к use case'у:

В `Order\Commands\CreateOrder` создадим `CreateOrderCommand`:

```csharp
public class CreateOrderCommand : IRequest<int>
{
    public CreateOrderDto Dto { get; set; }
}
```

И handler для него (тоже в папке `Order\Commands\CreateOrder`):

```csharp
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IMapper _mapper;
    private readonly IDbContext _dbContext;

    public CreateOrderCommandHandler(IMapper mapper, IDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Domain.Entities.Order>(command.Dto);
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order.Id;
    }
}
```

В метод `CreateOrderCommandHandler.Handle` попадает функционал из метода
`OrderService.CreateOrderAsync`.

4.3. Аналогично делаем внутренний request:

В `Order\Queries\GetById` создадим `GetOrderByIdQuery` и `GetOrderByIdQueryHandler`.

4.4. В `Order\Dto` переносим:

- `CreateOrderDto`
- `OrderDto`
- `CreateItemDto`

4.5. В `Order\Utils` перенесем `MapperProfile`

*Мое замечание: если будет mapping чего-то другого, помимо Order, то `Utils` надо ИМХО переместить в корень проекта `UseCases`*.

5. Удаляем `IOrderService` и `OrderService` - они уже больше не нужны.

6. В `WebApp`:

6.1. Регистрация `MediatR`

Вместо:

```csharp
builder.Services.AddScoped<IOrderService, OrderService>();
```

Будет:

```csharp
builder.Services.AddMediatR(typeof(CreateOrderCommand));
```

6.2. Контроллер `OrdersController` будет выглядеть так:

```csharp
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<OrderDto> Get(int id)
    {
        var result = await _sender.Send(new GetOrderByIdQuery { Id = id });
        return result;
    }

    [HttpPost]
    public async Task<int> Create([FromBody] CreateOrderDto dto)
    {
        var id = await _sender.Send(new CreateOrderCommand { Dto = dto });
        return id;
    }
}
```