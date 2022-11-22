# Анализ сэмпла чистой архитектуры от Jason Taylor

Source Git: `https://github.com/jasontaylordev/NorthwindTraders`

Fork Git: `https://github.com/denis-tsv/NorthwindTraders-CleanArchitecture`

## Обзор `Domain`

*Проект: 22. FromJasonTaylor*

Обзор структуры проекта снизу вверх. Самый нижний компонент `Domain`.

Здесь:

- Базовые классы `ValueObject`, `AuditableEntity`
- Entities
- Exceptions (относятся к предментной области)
- ValueObjects

Выводы:

- Дядя Боб называет подобного рода компоненты "Entity".
- Домен сделан хорошо.
- Нет интерфейсов инфраструктуры (репозитории).
- Entities используют анемичную модель.

### Добавление доменных сервисов `DomainServices`

*Проект: 23. FromJasonTaylorRefactor1*

Как легко можно добавить компонент `DomainServices`?

1. Создание новых проектов:

- `DomainServices.Interfaces` (интерфейсы)
- `DomainServices` (реализация)

2. Ссылки:

- `DomainServices.Interfaces` ссылается на `Domain`
- `DomainServices` ссылается на `DomainServices.Interfaces`
- `WebUI` ссылается на `DomainServices`

3. Регистрация в `WebUI`

В данном примере, в каждом компоненте есть свой регистратор.

3.1. Создание вспомогательного класса для регистрации зависимости. 

В `DomainServices` надо добавить nuget-пакет `Microsoft.Extensions.DependencyInjection.Abstractions`

Регистратор в `DomainServices`:

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderDomainService, OrderDomainService>();
        return services;
    }
}
```

3.2. Регистрация в `WebUI`

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainServices();
        //...
    }
```

## Обзор `Common`, `Application`

*Проект: 22. FromJasonTaylor*

*Проект: 23. FromJasonTaylorRefactor1*

Компонент `Common` содержит единственный интерфейс `IDateTime`. `Common` претендент на самый
нижний уровень иерархии.

Компонент `Application` напоминает слой `Use Cases`. Для реализации CQRS использует `MediatR`.

Содержит:

- CQRS (команды и хендлеры, запросы и хедлеры - находятся вместе, в одном файле). Папки:
  - `Categories`
  - `Customers`
  - `Employees`
  - `Products`
  - `System`

- Папка `Common`:
  - Папка `Behaviours` (вспомогательные классы, используемые в MediatR, для CQRS):
    - `RequestLogger`
    - `RequestPerformanceBehaviour`
    - `RequestValidationBehavior`
  - Папка `Exceptions` (исключения уровня Application):
    - `BadRequestException`
    - `DeleteFailureException`
    - `NotFoundException`
    - `ValidationException`
  - Папка `Interfaces`:
    - `ICsvFileBuilder`
    - `ICurrentUserService`
    - `INorthwindDbContext`
    - `INotificationService`
    - `IUserManager`
  - Папка `Mappings` (используется `AutoMapper`):
    - `IMapFrom`
    - `MappingProfile`
  - Папка `Models`:
    - `Result` (возвращаемый результат без данных, но с описанием ошибок)

### Перенос в `Common`

В `Application` класс `Result` "претендует" на более низкий уровень - перенесем его в `Common`

Добавление в `Domain` ссылку на `Common`.

### Новый проект `Infrastructure.Interfaces`

*Проект: 23. FromJasonTaylorRefactor1*

Из компонента `Application` в `Infrastructure.Interfaces` можно перенести содержимое папки
`Interfaces` - это интерфейсы инфраструктуры.

1. Создаем новый компонент `Infrastructure.Interfaces`

2. Добавляем nuget-пакет `Microsoft.EntityFrameworkCore`

3. Из `Infrastructure.Interfaces` ссылка на `Domain` и `Common`

4. Перенос почти всех интерфейсов из `Application.Interfaces` в `Infrastructure.Interfaces`

В `Application` остается только 1 интерфейс `ICsvFileBuilder`:

```csharp
namespace Northwind.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildProductsFile(IEnumerable<ProductRecordDto> records);
    }
}
```

Этот интерфейс содержит ссылку `ProductRecordDto` - кандидат на перенос в `ApplicationServices`
(будет сделано в следующих рефакторингах).

5. Перенос DTO из `Application` в `Infrastructure.Interfaces` для их использования в интерфейсах.

### Перенос проектов в папки

- Папка `0 Utils`
  - Проект `Common`
- Папка `1 Domain`
  - Проект `Domain`
  - Проект `DomainServices`
  - Проект `DomainServices.Infrastructure`
- Папка `2 Infrastructure Interfaces`
  - Проект `Infrastructure.Interfaces`
