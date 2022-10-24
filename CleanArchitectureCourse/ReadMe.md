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
